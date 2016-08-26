# Import SDK Dll.

Add-Type -Path C:\users\martincx\Source\Repos\TcgSdk\TcgSdk\TcgSdk\bin\Debug\TcgSdk.dll

function Get-TcgCards
{
    <#
    .SYNOPSIS
    Get cards utilizing the TcgSdk for pokemontcg.io and magicthegathering.io

    .PARAMETER CARDTYPE
    The TcgSdk.CardType of the card(s) you would like

    .PARAMETER FILTER
    The System.Collections.Generic.Dictionary[string,string] filter. The Key should be the name of the parameter, and the value should be the value of the parameter. For lists, use | for or and , for and.
    #>
    Param(
        [Parameter(
            Mandatory = $true,
            Position = 0)]
        [TcgSdk.Common.ITcgCardType] $CardType,

        [Parameter(
            Mandatory = $false,
            Position = 1)]
        [System.Collections.Generic.Dictionary[string,string]] $Filter = (New-Object -TypeName 'System.Collections.Generic.Dictionary[string,string]')
        )

    try
    {
        switch ($CardType)
        {
            ([TcgSdk.Common.ITcgCardType]::MagicTheGathering) 
            { 
                $cards = [TcgSdk.Magic.MagicCard]::Get($Filter)
            }
            ([TcgSdk.Common.ITcgCardType]::Pokemon) 
            { 
                $cards = [TcgSdk.Pokemon.PokemonCard]::Get($Filter)
            }
            Default 
            { 
                Write-Error -Exception (New-Object -TypeName System.ArgumentException -ArgumentList ("CardType $CardType not supported.")) -Message "CardType $CardType not supported. Terminating request."
            }
        }

        foreach ($card in $cards)
        {
            Write-Output -InputObject $card
        }
    }
    catch [System.Exception]
    {
        Write-Error -Message "There was a problem retrieving the requested cards. $($Error[0])"
    }
}

function Get-Hand
{
    <#
    .SYNOPSIS
    Get an opening hand of TCG cards

    .PARAMETER DECK
    The pool of cards to retrieve a hand from.
    #>
	Param(
		[Parameter(
			Mandatory = $true,
			Position = 0)]
        [TcgSdk.Common.ITcgCard[]] $Deck,

        [Parameter(
            Mandatory = $false,
            Position = 1)]
        [int] $HandSize = 7
		)

	$ran = New-Object -TypeName System.Random

    $myOpeningHandCards = @()
    
    for ($i = 0; $i -lt $HandSize; $i++) 
    {
        $cardNumber =  $ran.Next(0,60)

        while ($myOpeningHandCards.Contains($cardNumber))
        {
            $cardNumber = $ran.Next(0,60)
        }

        $myOpeningHandCards += $cardNumber
    }

    foreach ($cardNumber_ in $myOpeningHandCards)
    {
        Write-Output -InputObject $Deck[$cardNumber_]
    }
}