using Scrapper.Domain.Abstractions;

namespace Scrapper.Domain.Scrapes
{
    public sealed class Scrape : Entity<ScrapeId>
    {
        public int FrekNumber { get; private set; }
        public int AccountNumber { get; private set; }

        public string StmtCycle { get; private set; }
        public string Artist { get; private set; }
        public string FromDate { get; private set; }
        public string ToDate { get; private set; }
        public string ForeignLabel { get; private set; }
        public string ForeignProduct { get; private set; }
        public string ForeignCategory { get; private set; }
        public string CdoVerId { get; private set; }
        public string Units { get; private set; }
        public string RoyaltyBase { get; private set; }
        public string ArtistPercentage { get; private set; }
        public string RoyaltyRate { get; private set; }
        public string ArtistType { get; private set; }
        public string LicensePercentage { get; private set; }
        public string RoyaltyAmount { get; private set; }
        public string AfmLiability { get; private set; }
        public string PenyRate { get; private set; }
        public string Comments { get; private set; }
        public string PikdFlag { get; private set; }
        public string Ppd { get; private set; }
        public string OrigCycle { get; private set; }
    }
}
