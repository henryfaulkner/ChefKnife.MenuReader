using System;
using System.Collections.Generic;

namespace ChefKnife.Display.WebAPI.DTOs.MET;

public class ElementMeasurements
{
    public double Height { get; set; }
}

public class Measurements
{
    public string ElementName { get; set; }
    public string ElementDescription { get; set; }
    public ElementMeasurements ElementMeasurements { get; set; }
}

public class MetObject
{
    public int ObjectID { get; set; }
    public bool IsHighlight { get; set; }
    public string AccessionNumber { get; set; }
    public string AccessionYear { get; set; }
    public bool IsPublicDomain { get; set; }
    public string PrimaryImage { get; set; }
    public string PrimaryImageSmall { get; set; }
    public List<string> AdditionalImages { get; set; }
    public object Constituents { get; set; }
    public string Department { get; set; }
    public string ObjectName { get; set; }
    public string Title { get; set; }
    public string Culture { get; set; }
    public string Period { get; set; }
    public string Dynasty { get; set; }
    public string Reign { get; set; }
    public string Portfolio { get; set; }
    public string ArtistRole { get; set; }
    public string ArtistPrefix { get; set; }
    public string ArtistDisplayName { get; set; }
    public string ArtistDisplayBio { get; set; }
    public string ArtistSuffix { get; set; }
    public string ArtistAlphaSort { get; set; }
    public string ArtistNationality { get; set; }
    public string ArtistBeginDate { get; set; }
    public string ArtistEndDate { get; set; }
    public string ArtistGender { get; set; }
    public string ArtistWikidataURL { get; set; }
    public string ArtistULANURL { get; set; }
    public string ObjectDate { get; set; }
    public int ObjectBeginDate { get; set; }
    public int ObjectEndDate { get; set; }
    public string Medium { get; set; }
    public string Dimensions { get; set; }
    public List<Measurements> Measurements { get; set; }
    public string CreditLine { get; set; }
    public string GeographyType { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string County { get; set; }
    public string Country { get; set; }
    public string Region { get; set; }
    public string Subregion { get; set; }
    public string Locale { get; set; }
    public string Locus { get; set; }
    public string Excavation { get; set; }
    public string River { get; set; }
    public string Classification { get; set; }
    public string RightsAndReproduction { get; set; }
    public string LinkResource { get; set; }
    public DateTime MetadataDate { get; set; }
    public string Repository { get; set; }
    public string ObjectURL { get; set; }
    public object Tags { get; set; }
    public string ObjectWikidataURL { get; set; }
    public bool IsTimelineWork { get; set; }
    public string GalleryNumber { get; set; }
}

