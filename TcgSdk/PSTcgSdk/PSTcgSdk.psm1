# Import SDK Dll.

Add-Type -Path D:\TcgSdk\TcgSdk\TcgSdk\bin\Debug\TcgSdk.dll

$collectionPath = "C:\Users\Batman\Documents\TcgSdk\collection.json"

if (Test-Path -Path $collectionPath)
{
	$Global:TcgCardCollection = [TcgSdk.Common.Cards.ITcgCardCollection]::ImportCollection($collectionPath)
}
else
{
	$Global:TcgCardCollection = New-Object -TypeName 'TcgSdk.Common.Cards.ITcgCardCollection'
}

function Draw-ITcgCards
{
    <#
    .SYNOPSIS
    Draw a number of ITcgCards from a deck

    .PARAMETER DECK
    The pool of cards to retrieve a hand from.
    #>
	Param(
		[Parameter(
			Mandatory = $true,
			Position = 0,
            ParameterSetName = 'FROMOBJECT')]
        [TcgSdk.Common.Cards.ITcgCardDeck] $Deck,

		[Parameter(
			Mandatory = $true,
			Position = 0,
            ParameterSetName = 'BYNAME')]
        [string] $Name
		)

    if ($PSCmdlet.ParameterSetName -eq 'BYNAME')
    {
        try
        {
            $Deck = $Global:MyCurrentDecks[$Name]
        }
        catch
        {
            Write-Error -Exception $Error[0] -Message "Unable to find $Name in deck list."
            return
        }
    }

	$cards = $Deck.DrawCards(7)
}

function New-ITcgCardDeck
{
    [CmdletBinding()]
    Param(
        [Parameter(
            Mandatory = $true,
            Position = 0)]
        [string] $Name,

        [Parameter(
            Mandatory = $true,
            Position = 1)]
        [TcgSdk.Common.ITcgCard[]] $Cards
        )

    $cardHashSet = New-Object -TypeName 'System.Collections.Generic.HashSet[TcgSdk.Common.ITcgCard]' -ArgumentList (,$Cards)

	New-Object -TypeName 'TcgSdk.Common.ITcgCardDeck' -ArgumentList ($name, $cardHashSet)
    
}

function Add-ITcgCardDeck
{
    [CmdletBinding()]
    Param(
        [Parameter(
            Mandatory = $true,
            Position = 0,
            ParameterSetName = "FROMOBJECT")]
        [TcgSdk.Common.ITcgCardDeck] $Deck,

        [Parameter(
            Mandatory = $true,
            Position = 0,
            ParameterSetName = "NEW")]
        [string] $Name,

        [Parameter(
            Mandatory = $true,
            Position = 1,
            ParameterSetName = "NEW")]
        [TcgSdk.Common.ITcgCard[]] $Cards,

		[Parameter(
			ParameterSetName = "NEW")]
		[Parameter(
			ParameterSetName = "FROMOBJECT")]
		[switch] $FromCollection
        )

    function addDeck
    {
        Param($Deck_,$FromCollection)

        $Global:TcgCardCollection.AddDecks($Deck_, $FromCollection)
    }

    if ($PSCmdlet.ParameterSetName -eq 'NEW')
    {
        $Deck = New-ITcgCardDeck -Name $Name -Cards $Cards
    }
    
	addDeck -Deck_ $Deck -FromCollection $FromCollection
}

function Export-ITcgCardCollection
{
	[CmdletBinding()]
	Param(
		[Parameter(
			Mandatory = $false,
			Position = 0)]
		[string] $Path = $collectionPath
		)

	if ($Path -ne $collectionPath)
	{
		Write-Warning -Message "CollectionPath set to $collectionPath, and you are exporting to an alternate location. Collection will not automatically import upon module load."
	}

	$Global:TcgCardCollection.ExportCollection($Path)
}

function Import-ITcgCardCollection
{
	[CmdletBinding()]
	Param(
		[Parameter(
			Mandatory = $false,
			Position = 0)]
		[string] $Path = $collectionPath
		)
	
	if (!(Test-Path -Path $Path))
	{
		Write-Error -Exception (New-Object -TypeName System.ArgumentException -ArgumentList ("Path $Path not found")) -Message "Path $Path not found"
		return
	}

	[TcgSdk.Common.ITcgCardCollection]::ImportCollection($Path)
}

function New-ITcgFilterParameter
{
    Param(
        [Parameter(
            Mandatory = $true,
            Position = 0)]
        [string] $Name,

        [Parameter(
            Mandatory = $true,
            Position = 1)]
        [object] $Value,

        [switch] $UseAnd,
        [switch] $Multivalue
        )

    [TcgSdk.Common.ITcgCardRequestParameter]::New($Name, $Value, $UseAnd, $Multivalue)
}