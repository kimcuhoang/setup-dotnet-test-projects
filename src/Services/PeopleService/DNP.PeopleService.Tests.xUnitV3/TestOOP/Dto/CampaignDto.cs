using DNP.PeopleService.Tests.xUnitV3.TestOOP.Domain;

namespace DNP.PeopleService.Tests.xUnitV3.TestOOP.Dto;
internal class CampaignDto
{
    public Guid Id { get; init; }
    public IEnumerable<CampaignActivityHasGiftDto> HasGiftActivities { get; init; }
    public IEnumerable<CampaignActivityHasFunnyWordDto> HasFunnyWordActivities { get; init; }

    public static CampaignDto From(Campaign campaign)
    {
        var campaignDto = new CampaignDto
        {
            Id = campaign.Id,
            HasGiftActivities = campaign.Activities
                                        .OfType<CampaignActivitityHasGift>()
                                        .Select(_ => new CampaignActivityHasGiftDto().From(_)),
            HasFunnyWordActivities = campaign.Activities
                                        .OfType<CampaignActivityHasFunnyWord>()
                                        .Select(_ => new CampaignActivityHasFunnyWordDto().From(_))
        };
        return campaignDto;
    }
}