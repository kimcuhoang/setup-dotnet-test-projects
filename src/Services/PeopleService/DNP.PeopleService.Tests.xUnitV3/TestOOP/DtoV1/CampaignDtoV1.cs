using DNP.PeopleService.Tests.xUnitV3.TestOOP.Domain;

namespace DNP.PeopleService.Tests.xUnitV3.TestOOP.DtoV1;

internal abstract class DtoBase<TClass, TDto>
    where TClass : BaseEntity
    where TDto : DtoBase<TClass, TDto>
{
    public Guid Id { get; set; }

    public virtual TDto FromBase(TClass entity)
    {
        this.Id = entity.Id;
        return (TDto)this;
    }
}

internal abstract class DtoHasAuditBase<TClass, TDto> : DtoBase<TClass, TDto>
    where TClass : BaseEntityHasAudit
    where TDto : DtoHasAuditBase<TClass, TDto>
{
    public DateTime CreatedAt { get; set; }
    public override TDto FromBase(TClass entity)
    {
        base.FromBase(entity);
        this.CreatedAt = entity.CreatedAt;
        return (TDto)this;
    }
}

internal class CampaignDtoV1 : DtoHasAuditBase<Campaign, CampaignDtoV1>
{
    public IEnumerable<CampaignActivityHasGiftDtoV1> HasGiftActivities { get; set; }
    public IEnumerable<CampaignActivityHasFunnyWordDtoV1> HasFunnyWordActivities { get; set; }

    public override CampaignDtoV1 FromBase(Campaign entity)
    {
        base.FromBase(entity);

        this.HasGiftActivities = entity.Activities
                                    .OfType<CampaignActivityHasGift>()
                                    .Select(_ => new CampaignActivityHasGiftDtoV1().FromBase(_));

        this.HasFunnyWordActivities = entity.Activities
                                    .OfType<CampaignActivityHasFunnyWord>()
                                    .Select(_ => new CampaignActivityHasFunnyWordDtoV1().FromBase(_));

        return this;
    }
}