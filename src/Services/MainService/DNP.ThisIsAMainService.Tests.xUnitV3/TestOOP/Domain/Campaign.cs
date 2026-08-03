namespace DNP.PeopleService.Tests.xUnitV3.TestOOP.Domain;

internal abstract class BaseEntity
{
    public Guid Id { get; internal set; }
}

internal abstract class BaseEntityHasAudit : BaseEntity
{
    public DateTime CreatedAt { get; internal set; }
}

internal class Campaign: BaseEntityHasAudit
{
    private readonly List<CampaignActivity> _activities = [];
    public IEnumerable<CampaignActivity> Activities => this._activities.AsReadOnly();

    public T SetupActivity<T>(Guid id) where T : CampaignActivity, new()
    {
        var activity = new T
        {
            Id = id,
            Campaign = this,
            CampaignId = this.Id,
            CreatedAt = this.CreatedAt
        };
        this._activities.Add(activity);
        return activity;
    }
}
