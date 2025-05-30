using DNP.PeopleService.Tests.xUnitV3.TestOOP.Domain;

namespace DNP.PeopleService.Tests.xUnitV3.TestOOP.DtoV1;
internal class CampaignDtoV1
{
    public Guid Id { get; init; }
    public IEnumerable<CampaignActivityHasGiftDtoV1> HasGiftActivities { get; init; }
    public IEnumerable<CampaignActivityHasFunnyWordDtoV1> HasFunnyWordActivities { get; init; }

    public static CampaignDtoV1 From(Campaign campaign)
    {
        var campaignDto = new CampaignDtoV1
        {
            Id = campaign.Id,
            HasGiftActivities = campaign.Activities
                                        .OfType<CampaignActivityHasGift>()
                                        .Select(_ => new CampaignActivityHasGiftDtoV1().FromBase(_)),
            HasFunnyWordActivities = campaign.Activities
                                        .OfType<CampaignActivityHasFunnyWord>()
                                        .Select(_ => new CampaignActivityHasFunnyWordDtoV1().FromBase(_))
        };
        return campaignDto;
    }
}