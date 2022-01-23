using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamblingDen : Dialogue
{
    public string GetGamblingDenName()
    {

        string[] ownerNames = {
            "Big Zach's",
            "Little Jackson's",
            "Borgo Depwazit's",
            "Funkman's",
            "Grungus'"
        };

        string[] placeNames = {
            "Den",
            "Zone",
            "Emporium",
            "Place"
        };

        string[] adjectives = {
            "Hellacious",
            "Endless",
            "Tasteless",
            "Paramount",
            "Enticing",
            "Slovenly",
            "Triangular",
            "Feckless"
        };

        string[] vices = {
            "Depravity",
            "Greed",
            "Debauchery",
            "Gamery",
            "Idiocy"
        };

        string[] activities = {
            "Party",
            "Gamble",
            "Waste and Want",
            "Fritter and Forget",
            "Hope and Mourn",
            "Lose. Badly."
        };

        string ownerName = ownerNames[Random.Range(0, ownerNames.Length)];
        string placeName = placeNames[Random.Range(0, placeNames.Length)];

        if (Random.Range(0,2) == 1)
        {
            string adjective = adjectives[Random.Range(0, adjectives.Length)];
            string vice = vices[Random.Range(0, vices.Length)];
            return ownerName + " " + placeName + " of " + adjective + " " + vice;
        }
        else
        {
            string activity = activities[Random.Range(0, activities.Length)];
            return ownerName + " " + placeName + ": the Place to " + activity;
        }

    }
}
