namespace DNP.PeopleService.Tests.xUnitV3.TestOOP.Domain;

internal enum CampaignActivityType { HasGift, HasFunnyWord }

internal abstract class CampaignActivity : BaseEntityHasAudit
{
    public abstract CampaignActivityType ActivityType { get; protected set; }
    public Campaign Campaign { get; internal set; }
    public Guid CampaignId { get; internal set; }
}

internal sealed class CampaignActivityHasGift : CampaignActivity
{
    public override CampaignActivityType ActivityType { get; protected set; } = CampaignActivityType.HasGift;

    public List<string> Gifts { get; set; } = [];
}

internal sealed class CampaignActivityHasFunnyWord : CampaignActivity
{
    public override CampaignActivityType ActivityType { get; protected set; } = CampaignActivityType.HasFunnyWord;

    public string FunnyWord { get; set; }
}
