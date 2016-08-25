# Import SDK Dll.

Add-Type -Path C:\users\martincx\Source\Repos\TcgSdk\TcgSdk\TcgSdk\bin\Debug\TcgSdk.dll

$Global:MagicApiUrl = "https://api.magicthegathering.io/v1/cards"
$Global:PokemonApiUrl = "https://api.pokemontcg.io/v1/cards/"

function Get-TcgCards
{
    Param(
        [Parameter(
            Mandatory = $true,
            Position = 0)]
        [TcgSdk.CardType] $CardType,

        [Parameter(
            Mandatory = $false,
            Position = 1)]
        [string] $Filter = ""
        )
    try
    {
        switch ($CardType)
        {
            ([TcgSdk.CardType]::MagicTheGathering) 
            { 
                Write-Error -Exception (New-Object -TypeName System.ArgumentException -ArgumentList ("CardType $CardType not supported.")) -Message "CardType $CardType not supported. Terminating request."
            }
            ([TcgSdk.CardType]::Pokemon) 
            { 
                $cards = [TcgSdk.Pokemon.PokemonCard]::Get($Filter)

                foreach ($card in $cards)
                {
                    Write-Output -InputObject $card
                }
            }
            Default 
            { 
                Write-Error -Exception (New-Object -TypeName System.ArgumentException -ArgumentList ("CardType $CardType not supported.")) -Message "CardType $CardType not supported. Terminating request."
            }
        }
    }
    catch [System.Exception]
    {
        Write-Error -Message "There was a problem retrieving the requested cards. $($Error[0])"
    }
}

function Simulate-OpeningHand
{
	Param(
		[Parameter(
			Mandatory = $true,
			Position = 0)]
        [TcgSdk.ITcgCard[]] $Deck
		)

	$ran = New-Object -TypeName System.Random

    $myOpeningHandCards = @()
    
    for ($i = 1; $i -lt 8; $i++) 
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