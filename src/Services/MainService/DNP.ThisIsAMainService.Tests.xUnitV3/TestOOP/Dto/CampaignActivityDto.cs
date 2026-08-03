using DNP.PeopleService.Tests.xUnitV3.TestOOP.Domain;

namespace DNP.PeopleService.Tests.xUnitV3.TestOOP.Dto;

internal abstract class CampaignActivityDto<T> where T : CampaignActivity
{
    public Guid Id { get; set; }
    public CampaignActivityType ActivityType { get; set; }
    protected void FromBase(T activity)
    {
        this.Id = activity.Id;
        this.ActivityType = activity.ActivityType;
    }
}

internal sealed class CampaignActivityHasGiftDto : CampaignActivityDto<CampaignActivityHasGift>
{
    public List<string> Gifts { get; set; } = [];

    public CampaignActivityHasGiftDto From(CampaignActivityHasGift activity)
    {
        this.FromBase(activity);
        this.Gifts = activity.Gifts;
        return this;
    }
}

internal sealed class CampaignActivityHasFunnyWordDto : CampaignActivityDto<CampaignActivityHasFunnyWord>
{
    public string FunnyWord { get; set; }

    public CampaignActivityHasFunnyWordDto From(CampaignActivityHasFunnyWord activity)
    {
        this.FromBase(activity);
        this.FunnyWord = activity.FunnyWord;
        return this;
    }
}