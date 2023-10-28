using Scrapper.Domain.Abstractions;

namespace Scrapper.Domain.Royalties
{
    public sealed class Scrape : Entity<ScrapeId>
    {
        // Private constructor so as to create it using Factory method

        //private Royalty(RoyaltyId id, int frekNumber, int accountNumber, string stmtCycle, string artist, Country country, DateRange duration, string foreignLabel, string foreignProduct, string foreignCategory, string cdoVerId, string units, string royaltyBase, string artistPercentage, string royaltyRate, string artistType, string licensePercentage, string royaltyAmount, string afmLiability, string penyRate, string comments, string pikdFlag, string ppd, string origCycle) : base(id)
        //{
        //    FrekNumber = frekNumber;
        //    AccountNumber = accountNumber;
        //    StmtCycle = stmtCycle;
        //    Artist = artist;
        //    Country = country;
        //    Duration = duration;
        //    ForeignLabel = foreignLabel;
        //    ForeignProduct = foreignProduct;
        //    ForeignCategory = foreignCategory;
        //    CdoVerId = cdoVerId;
        //    Units = units;
        //    RoyaltyBase = royaltyBase;
        //    ArtistPercentage = artistPercentage;
        //    RoyaltyRate = royaltyRate;
        //    ArtistType = artistType;
        //    LicensePercentage = licensePercentage;
        //    RoyaltyAmount = royaltyAmount;
        //    AfmLiability = afmLiability;
        //    PenyRate = penyRate;
        //    Comments = comments;
        //    PikdFlag = pikdFlag;
        //    Ppd = ppd;
        //    OrigCycle = origCycle;
        //}

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
