namespace DNP.PeopleService.Tests.xUnitV3.TestOOP.Domain;
internal class Campaign(Guid id)
{
    public Guid Id { get; private set; } = id;

    private readonly List<CampaignActivity> _activities = [];
    public IEnumerable<CampaignActivity> Activities => this._activities.AsReadOnly();

    public T SetupActivity<T>(Guid id) where T: CampaignActivity, new()
    {
        var activity = new T
        {
            Id = id,
            Campaign = this,
            CampaignId = this.Id
        };
        this._activities.Add(activity);
        return activity;
    }
}
