using DNP.PeopleService.Tests.xUnitV3.TestOOP.Domain;
using DNP.PeopleService.Tests.xUnitV3.TestOOP.Dto;
using DNP.PeopleService.Tests.xUnitV3.TestOOP.DtoV1;

namespace DNP.PeopleService.Tests.xUnitV3.TestOOP;
public class TestSetupWithOOP
{
    [Fact]
    public void ShouldBeCorrect()
    {
        var campaign = new Campaign
        {
            Id = Guid.CreateVersion7(),
            CreatedAt = DateTime.UtcNow
        };

        var activityHasGift = campaign.SetupActivity<CampaignActivityHasGift>(Guid.NewGuid());
        activityHasGift.Gifts.Add("Gift1");
        activityHasGift.Gifts.Add("Gift2");

        var activityHasFunnyWord = campaign.SetupActivity<CampaignActivityHasFunnyWord>(Guid.NewGuid());
        activityHasFunnyWord.FunnyWord = "FunnyWord1";

        var campaignDto = CampaignDto.From(campaign);
        campaignDto.ShouldSatisfyAllConditions(_ =>
        {
            _.Id.ShouldBe(campaign.Id);
            _.HasGiftActivities.ShouldSatisfyAllConditions(_ =>
                    {
                        _.Count().ShouldBe(1);
                        _.First().ShouldSatisfyAllConditions(activity =>
                        {
                            activity.Id.ShouldBe(activityHasGift.Id);
                            activity.ActivityType.ShouldBe(CampaignActivityType.HasGift);
                            activity.Gifts.ShouldSatisfyAllConditions(gifts =>
                                {
                                    gifts.Count.ShouldBe(2);
                                    gifts.ShouldContain("Gift1");
                                    gifts.ShouldContain("Gift2");
                                });
                        });
                    });
            _.HasFunnyWordActivities.ShouldSatisfyAllConditions(_ =>
                    {
                        _.Count().ShouldBe(1);
                        _.First().ShouldSatisfyAllConditions(activity =>
                        {
                            activity.Id.ShouldBe(activityHasFunnyWord.Id);
                            activity.ActivityType.ShouldBe(CampaignActivityType.HasFunnyWord);
                            activity.FunnyWord.ShouldBe("FunnyWord1");
                        });
                    });

        });
    }

    [Fact]
    public void ShouldBeCorrectV1()
    {
        var campaign = new Campaign 
        { 
            Id = Guid.CreateVersion7(),
            CreatedAt = DateTime.UtcNow
        };

        var activityHasGift = campaign.SetupActivity<CampaignActivityHasGift>(Guid.NewGuid());
        activityHasGift.Gifts.Add("Gift1");
        activityHasGift.Gifts.Add("Gift2");

        var activityHasFunnyWord = campaign.SetupActivity<CampaignActivityHasFunnyWord>(Guid.NewGuid());
        activityHasFunnyWord.FunnyWord = "FunnyWord1";

        var campaignDto = new CampaignDtoV1().FromBase(campaign);
        this.CompareDtoV1WithEntity(campaignDto, campaign);

        var toJson = JsonSerializer.Serialize(campaignDto);
        var campaignDtoFromJson = JsonSerializer.Deserialize<CampaignDtoV1>(toJson);
        campaignDtoFromJson.ShouldNotBeNull();
        this.CompareDtoV1WithEntity(campaignDtoFromJson, campaign);
    }

    private void CompareDtoV1WithEntity(CampaignDtoV1 campaignDto, Campaign campaign)
    {
        campaignDto.Id.ShouldBe(campaign.Id);
        campaignDto.CreatedAt.ShouldBe(campaign.CreatedAt);
        campaignDto.HasGiftActivities.ShouldSatisfyAllConditions(_ =>
        {
            _.Count().ShouldBe(campaign.Activities.OfType<CampaignActivityHasGift>().Count());
            _.First().ShouldSatisfyAllConditions(activity =>
            {
                var entityActivity = campaign.Activities.OfType<CampaignActivityHasGift>().First();
                activity.Id.ShouldBe(entityActivity.Id);
                activity.ActivityType.ShouldBe(CampaignActivityType.HasGift);
                activity.CreatedAt.ShouldBe(entityActivity.CreatedAt);
                activity.Gifts.ShouldSatisfyAllConditions(gifts =>
                {
                    gifts.Count.ShouldBe(entityActivity.Gifts.Count);
                    foreach (var gift in entityActivity.Gifts)
                    {
                        gifts.ShouldContain(gift);
                    }
                });
            });
        });
        campaignDto.HasFunnyWordActivities.ShouldSatisfyAllConditions(_ =>
        {
            _.Count().ShouldBe(campaign.Activities.OfType<CampaignActivityHasFunnyWord>().Count());
            _.First().ShouldSatisfyAllConditions(activity =>
            {
                var entityActivity = campaign.Activities.OfType<CampaignActivityHasFunnyWord>().First();
                activity.Id.ShouldBe(entityActivity.Id);
                activity.ActivityType.ShouldBe(CampaignActivityType.HasFunnyWord);
                activity.CreatedAt.ShouldBe(entityActivity.CreatedAt);
                activity.FunnyWord.ShouldBe(entityActivity.FunnyWord);
            });
        });
    }
}
