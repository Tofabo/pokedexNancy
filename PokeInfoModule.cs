using Nancy;
using System;
using System.Collections.Generic;
using ApiCaller;
using Newtonsoft.Json.Linq;

namespace PokeInfo
{
    public class PokeInfoModule : NancyModule
    {
        public PokeInfoModule()
        {
            Get("/", args =>
                {
                return View["home"];
                });

            Get("/{id}", args => {
                ViewBag.PassedId = args.id;
                string requestURL = "http://pokeapi.co/api/v2/pokemon/" + args.id;
                List<string> abilities = new List<string>();
                WebRequest.SendRequest(requestURL, new Action<Dictionary<string,object>>(JsonResponse => {
                    string Name = (string)JsonResponse["name"];
                    Int64 Height = (int)(long)JsonResponse["height"];
                    Int64 Weight = (int)(long)JsonResponse["weight"];
                    string Ability = (string)((JArray)JsonResponse["abilities"])[0]["ability"]["name"];
                    // object[] abilitiesArray = (object[])((JArray)JsonResponse["abilities"]);
                    // public List<string> abilities = new List<string>();
                    foreach(var ability in (JArray)JsonResponse["abilities"]){ //why doesn't it recognize this as a for loop when there is a public variable??
                        // System.Console.WriteLine(ability["ability"]["name"]);
                        abilities.Add((string)ability["ability"]["name"]); //variable scoping.....interesting
                    }
                    ViewBag.Abilities = abilities;
                    // foreach( var thing in ViewBag.Abilities){
                    //     System.Console.WriteLine(thing);
                    // }
                    // System.Console.WriteLine(ViewBag.Abilities);
                    // System.Console.WriteLine((string)((JArray)JsonResponse["abilities"])[1]["ability"]["name"]);
                    string Type = (string)((JArray)JsonResponse["types"])[0]["type"]["name"];
                    ViewBag.PokeInfo = "Name: " + Name + ", ID: #"+ args.id + ", Primary Type: " + Type + ", Weight: " + Weight + ", Height: " + Height + " and it's abilities are: ";
                    // var test = JsonResponse["type"];
                    // System.Console.WriteLine(test); //see the type of what is returned, some strange array of objects!
                    // var test = (JArray)JsonResponse["types"];
                    // System.Console.WriteLine(test[0]["type"]["name"]); //returns flying

                    //interesting project idea: JSON parser to loook at JSon objects and return an object wth type already parsed....to any static  language
                })); 
                string picURL = "http://pokeapi.co/api/v1/pokemon/" + args.id;
                WebRequest.SendRequest(picURL, new Action<Dictionary<string,object>>(JsonResponse => {   
                    string spriteID = (string)((JArray)JsonResponse["sprites"])[0]["resource_uri"];
                    System.Console.WriteLine(spriteID);
                })); 

                return View["InfoPage", abilities];
            });
        }
    }
}