using DNP.PeopleService.Tests.xUnitV3.TestOOP.Domain;

namespace DNP.PeopleService.Tests.xUnitV3.TestOOP.DtoV1;

internal abstract class CampaignActivityDtoV1<TClass, TDto> : DtoHasAuditBase<TClass, TDto>
        where TClass : CampaignActivity
        where TDto : CampaignActivityDtoV1<TClass, TDto>
{
    public CampaignActivityType ActivityType { get; set; }

    public override TDto FromBase(TClass activity)
    {
        base.FromBase(activity);
        this.ActivityType = activity.ActivityType;
        return (TDto)this;
    }
}

internal sealed class CampaignActivityHasGiftDtoV1 : CampaignActivityDtoV1<CampaignActivityHasGift, CampaignActivityHasGiftDtoV1>
{
    public List<string> Gifts { get; set; } = [];

    public override CampaignActivityHasGiftDtoV1 FromBase(CampaignActivityHasGift activity)
    {
        base.FromBase(activity);
        this.Gifts = activity.Gifts;
        return this;
    }
}

internal sealed class CampaignActivityHasFunnyWordDtoV1 : CampaignActivityDtoV1<CampaignActivityHasFunnyWord, CampaignActivityHasFunnyWordDtoV1>
{
    public string FunnyWord { get; set; }

    public override CampaignActivityHasFunnyWordDtoV1 FromBase(CampaignActivityHasFunnyWord activity)
    {
        base.FromBase(activity);
        this.FunnyWord = activity.FunnyWord;
        return this;
    }
}