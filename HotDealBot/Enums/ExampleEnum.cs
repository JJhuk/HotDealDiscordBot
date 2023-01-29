using Discord.Interactions;

namespace HotDealBot.Enums;

public enum ExampleEnum
{
    First,
    Second,
    Third,
    Fourth,
    [ChoiceDisplay("Twenty First")]
    TwentyFirst
}