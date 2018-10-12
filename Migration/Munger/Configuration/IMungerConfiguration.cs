namespace Munger.Configuration
{
    public interface IMungerConfiguration
    {
        HostListElement HostList { get; }
        RewritingListElement ProgrammaticLinkList { get; }
        SingleValueElement RootElementPath { get; }
        RewritingListElement SubstitutionList { get; }
    }
}