using DNP.PeopleService.Tests.xUnitV3.TestOOP.Domain;
using DNP.PeopleService.Tests.xUnitV3.TestOOP.Dto;
using DNP.PeopleService.Tests.xUnitV3.TestOOP.DtoV1;

namespace DNP.PeopleService.Tests.xUnitV3.TestOOP;
public class TestSetupWithOOP
{
    [Fact]
    public void ShouldBeCorrect()
    {
        var campaign = new Campaign(Guid.NewGuid());

        var activityHasGift = campaign.SetupActivity<CampaignActivityHasGift>(Guid.NewGuid());
        activityHasGift.Gifts.Add("Gift1");
        activityHasGift.Gifts.Add("Gift2");

        var activityHasFunnyWord = campaign.SetupActivity<CampaignActivityHasFunnyWord>(Guid.NewGuid());
        activityHasFunnyWord.FunnyWord = "FunnyWord1";

        var campaignDto = CampaignDto.From(campaign);
        campaignDto.ShouldSatisfyAllConditions
        (
            () => campaignDto.Id.ShouldBe(campaign.Id),
            () => campaignDto.HasGiftActivities.ShouldSatisfyAllConditions
                    (
                        activities => activities.Count().ShouldBe(1),
                        activities => activities.First().ShouldSatisfyAllConditions
                        (
                            activity => activity.Id.ShouldBe(activityHasGift.Id),
                            activity => activity.ActivityType.ShouldBe(CampaignActivityType.HasGift),
                            activity => activity.Gifts.ShouldSatisfyAllConditions
                                (
                                    gifts => gifts.Count.ShouldBe(2), 
                                    gifts => gifts.ShouldContain("Gift1"), 
                                    gifts => gifts.ShouldContain("Gift2")
                                )
                        )
                    ),
            () => campaignDto.HasFunnyWordActivities.ShouldSatisfyAllConditions
                    (
                        activities => activities.Count().ShouldBe(1),
                        activities => activities.First().ShouldSatisfyAllConditions
                        (
                            activity => activity.Id.ShouldBe(activityHasFunnyWord.Id),
                            activity => activity.ActivityType.ShouldBe(CampaignActivityType.HasFunnyWord),
                            activity => activity.FunnyWord.ShouldBe("FunnyWord1")
                        )
                    )

        );
    }

    [Fact]
    public void ShouldBeCorrectV1()
    {
        var campaign = new Campaign(Guid.NewGuid());

        var activityHasGift = campaign.SetupActivity<CampaignActivityHasGift>(Guid.NewGuid());
        activityHasGift.Gifts.Add("Gift1");
        activityHasGift.Gifts.Add("Gift2");

        var activityHasFunnyWord = campaign.SetupActivity<CampaignActivityHasFunnyWord>(Guid.NewGuid());
        activityHasFunnyWord.FunnyWord = "FunnyWord1";

        var campaignDto = CampaignDtoV1.From(campaign);
        campaignDto.ShouldSatisfyAllConditions
        (
            () => campaignDto.Id.ShouldBe(campaign.Id),
            () => campaignDto.HasGiftActivities.ShouldSatisfyAllConditions
                    (
                        activities => activities.Count().ShouldBe(1),
                        activities => activities.First().ShouldSatisfyAllConditions
                        (
                            activity => activity.Id.ShouldBe(activityHasGift.Id),
                            activity => activity.ActivityType.ShouldBe(CampaignActivityType.HasGift),
                            activity => activity.Gifts.ShouldSatisfyAllConditions
                                (
                                    gifts => gifts.Count.ShouldBe(2),
                                    gifts => gifts.ShouldContain("Gift1"),
                                    gifts => gifts.ShouldContain("Gift2")
                                )
                        )
                    ),
            () => campaignDto.HasFunnyWordActivities.ShouldSatisfyAllConditions
                    (
                        activities => activities.Count().ShouldBe(1),
                        activities => activities.First().ShouldSatisfyAllConditions
                        (
                            activity => activity.Id.ShouldBe(activityHasFunnyWord.Id),
                            activity => activity.ActivityType.ShouldBe(CampaignActivityType.HasFunnyWord),
                            activity => activity.FunnyWord.ShouldBe("FunnyWord1")
                        )
                    )

        );
    }
}
