import sqlite3
import csv

def create_database(db_name="met.db"):
    """
    Create a SQLite database and table for storing Record objects.
    """
    conn = sqlite3.connect(db_name)
    cursor = conn.cursor()

    # Create table based on Record attributes
    cursor.execute('''
        CREATE TABLE IF NOT EXISTS records (
            Object_Number TEXT,
            Is_Highlight TEXT,
            Is_Timeline_Work TEXT,
            Is_Public_Domain TEXT,
            Object_ID TEXT,
            Gallery_Number TEXT,
            Department TEXT,
            AccessionYear TEXT,
            Object_Name TEXT,
            Title TEXT,
            Culture TEXT,
            Period TEXT,
            Dynasty TEXT,
            Reign TEXT,
            Portfolio TEXT,
            Constituent_ID TEXT,
            Artist_Role TEXT,
            Artist_Prefix TEXT,
            Artist_Display_Name TEXT,
            Artist_Display_Bio TEXT,
            Artist_Suffix TEXT,
            Artist_Alpha_Sort TEXT,
            Artist_Nationality TEXT,
            Artist_Begin_Date TEXT,
            Artist_End_Date TEXT,
            Artist_Gender TEXT,
            Artist_ULAN_URL TEXT,
            Artist_Wikidata_URL TEXT,
            Object_Date TEXT,
            Object_Begin_Date TEXT,
            Object_End_Date TEXT,
            Medium TEXT,
            Dimensions TEXT,
            Credit_Line TEXT,
            Geography_Type TEXT,
            City TEXT,
            State TEXT,
            County TEXT,
            Country TEXT,
            Region TEXT,
            Subregion TEXT,
            Locale TEXT,
            Locus TEXT,
            Excavation TEXT,
            River TEXT,
            Classification TEXT,
            Rights_and_Reproduction TEXT,
            Link_Resource TEXT,
            Object_Wikidata_URL TEXT,
            Metadata_Date TEXT,
            Repository TEXT,
            Tags TEXT,
            Tags_AAT_URL TEXT,
            Tags_Wikidata_URL TEXT
        )
    ''')

    conn.commit()
    conn.close()


def insert_record(record, db_name="met.db"):
    """
    Insert a single Record object into the SQLite database.
    """
    conn = sqlite3.connect(db_name)
    cursor = conn.cursor()

    # Prepare SQL insert statement
    cursor.execute('''
        INSERT INTO records VALUES (
            ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?
        )
    ''', (
        record.Object_Number,
        record.Is_Highlight,
        record.Is_Timeline_Work,
        record.Is_Public_Domain,
        record.Object_ID,
        record.Gallery_Number,
        record.Department,
        record.AccessionYear,
        record.Object_Name,
        record.Title,
        record.Culture,
        record.Period,
        record.Dynasty,
        record.Reign,
        record.Portfolio,
        record.Constituent_ID,
        record.Artist_Role,
        record.Artist_Prefix,
        record.Artist_Display_Name,
        record.Artist_Display_Bio,
        record.Artist_Suffix,
        record.Artist_Alpha_Sort,
        record.Artist_Nationality,
        record.Artist_Begin_Date,
        record.Artist_End_Date,
        record.Artist_Gender,
        record.Artist_ULAN_URL,
        record.Artist_Wikidata_URL,
        record.Object_Date,
        record.Object_Begin_Date,
        record.Object_End_Date,
        record.Medium,
        record.Dimensions,
        record.Credit_Line,
        record.Geography_Type,
        record.City,
        record.State,
        record.County,
        record.Country,
        record.Region,
        record.Subregion,
        record.Locale,
        record.Locus,
        record.Excavation,
        record.River,
        record.Classification,
        record.Rights_and_Reproduction,
        record.Link_Resource,
        record.Object_Wikidata_URL,
        record.Metadata_Date,
        record.Repository,
        record.Tags,
        record.Tags_AAT_URL,
        record.Tags_Wikidata_URL
    ))

    conn.commit()
    conn.close()

def map_text_to_record(text):
    """
    Map a comma-separated text string to a Record object.

    Parameters:
        text (str): Comma-separated values representing a single record.

    Returns:
        Record: The corresponding Record object.
    """
    fields = text.split(",")
    return Record(
        Object_Number=fields[0],
        Is_Highlight=fields[1],
        Is_Timeline_Work=fields[2],
        Is_Public_Domain=fields[3],
        Object_ID=fields[4],
        Gallery_Number=fields[5],
        Department=fields[6],
        AccessionYear=fields[7],
        Object_Name=fields[8],
        Title=fields[9],
        Culture=fields[10],
        Period=fields[11],
        Dynasty=fields[12],
        Reign=fields[13],
        Portfolio=fields[14],
        Constituent_ID=fields[15],
        Artist_Role=fields[16],
        Artist_Prefix=fields[17],
        Artist_Display_Name=fields[18],
        Artist_Display_Bio=fields[19],
        Artist_Suffix=fields[20],
        Artist_Alpha_Sort=fields[21],
        Artist_Nationality=fields[22],
        Artist_Begin_Date=fields[23],
        Artist_End_Date=fields[24],
        Artist_Gender=fields[25],
        Artist_ULAN_URL=fields[26],
        Artist_Wikidata_URL=fields[27],
        Object_Date=fields[28],
        Object_Begin_Date=fields[29],
        Object_End_Date=fields[30],
        Medium=fields[31],
        Dimensions=fields[32],
        Credit_Line=fields[33],
        Geography_Type=fields[34],
        City=fields[35],
        State=fields[36],
        County=fields[37],
        Country=fields[38],
        Region=fields[39],
        Subregion=fields[40],
        Locale=fields[41],
        Locus=fields[42],
        Excavation=fields[43],
        River=fields[44],
        Classification=fields[45],
        Rights_and_Reproduction=fields[46],
        Link_Resource=fields[47],
        Object_Wikidata_URL=fields[48],
        Metadata_Date=fields[49],
        Repository=fields[50],
        Tags=fields[51],
        Tags_AAT_URL=fields[52],
        Tags_Wikidata_URL=fields[53]
    )

class Record:
    """
    Class to represent a row in the CSV with predefined properties.
    """
    def __init__(
        self,
        Object_Number=None,
        Is_Highlight=None,
        Is_Timeline_Work=None,
        Is_Public_Domain=None,
        Object_ID=None,
        Gallery_Number=None,
        Department=None,
        AccessionYear=None,
        Object_Name=None,
        Title=None,
        Culture=None,
        Period=None,
        Dynasty=None,
        Reign=None,
        Portfolio=None,
        Constituent_ID=None,
        Artist_Role=None,
        Artist_Prefix=None,
        Artist_Display_Name=None,
        Artist_Display_Bio=None,
        Artist_Suffix=None,
        Artist_Alpha_Sort=None,
        Artist_Nationality=None,
        Artist_Begin_Date=None,
        Artist_End_Date=None,
        Artist_Gender=None,
        Artist_ULAN_URL=None,
        Artist_Wikidata_URL=None,
        Object_Date=None,
        Object_Begin_Date=None,
        Object_End_Date=None,
        Medium=None,
        Dimensions=None,
        Credit_Line=None,
        Geography_Type=None,
        City=None,
        State=None,
        County=None,
        Country=None,
        Region=None,
        Subregion=None,
        Locale=None,
        Locus=None,
        Excavation=None,
        River=None,
        Classification=None,
        Rights_and_Reproduction=None,
        Link_Resource=None,
        Object_Wikidata_URL=None,
        Metadata_Date=None,
        Repository=None,
        Tags=None,
        Tags_AAT_URL=None,
        Tags_Wikidata_URL=None
    ):
        self.Object_Number = Object_Number
        self.Is_Highlight = Is_Highlight
        self.Is_Timeline_Work = Is_Timeline_Work
        self.Is_Public_Domain = Is_Public_Domain
        self.Object_ID = Object_ID
        self.Gallery_Number = Gallery_Number
        self.Department = Department
        self.AccessionYear = AccessionYear
        self.Object_Name = Object_Name
        self.Title = Title
        self.Culture = Culture
        self.Period = Period
        self.Dynasty = Dynasty
        self.Reign = Reign
        self.Portfolio = Portfolio
        self.Constituent_ID = Constituent_ID
        self.Artist_Role = Artist_Role
        self.Artist_Prefix = Artist_Prefix
        self.Artist_Display_Name = Artist_Display_Name
        self.Artist_Display_Bio = Artist_Display_Bio
        self.Artist_Suffix = Artist_Suffix
        self.Artist_Alpha_Sort = Artist_Alpha_Sort
        self.Artist_Nationality = Artist_Nationality
        self.Artist_Begin_Date = Artist_Begin_Date
        self.Artist_End_Date = Artist_End_Date
        self.Artist_Gender = Artist_Gender
        self.Artist_ULAN_URL = Artist_ULAN_URL
        self.Artist_Wikidata_URL = Artist_Wikidata_URL
        self.Object_Date = Object_Date
        self.Object_Begin_Date = Object_Begin_Date
        self.Object_End_Date = Object_End_Date
        self.Medium = Medium
        self.Dimensions = Dimensions
        self.Credit_Line = Credit_Line
        self.Geography_Type = Geography_Type
        self.City = City
        self.State = State
        self.County = County
        self.Country = Country
        self.Region = Region
        self.Subregion = Subregion
        self.Locale = Locale
        self.Locus = Locus
        self.Excavation = Excavation
        self.River = River
        self.Classification = Classification
        self.Rights_and_Reproduction = Rights_and_Reproduction
        self.Link_Resource = Link_Resource
        self.Object_Wikidata_URL = Object_Wikidata_URL
        self.Metadata_Date = Metadata_Date
        self.Repository = Repository
        self.Tags = Tags
        self.Tags_AAT_URL = Tags_AAT_URL
        self.Tags_Wikidata_URL = Tags_Wikidata_URL

    def __repr__(self):
        return f"{self.__class__.__name__}({self.__dict__})"

# Example usage
if __name__ == "__main__":
    # Specify the path to your CSV file
    csv_file_path = "assets/MetObjects.txt"

    records = []

    # Map a text string to a Record object # Open the CSV file
    with open(csv_file_path, mode='r', encoding='utf-8') as file:
        line = file.readline()
        while line:
            try:
                record = map_text_to_record(line)
                records.append(record)
            except Exception as e:
                print(f"An file reader error occurred: {e}")
            finally:
                line = file.readline()

    # Create the database and table
    create_database()

    for record in records:
        try:
            insert_record(record)
        except Exception as e:
            print(f"An db error occurred: {e}")