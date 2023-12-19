using Spectre.Console;

namespace camp_sleepaway
{
    internal class Program
    {
        // Samuel Lööf, Simon Sörqvist, Adam Kumlin
        static void Main()
        {
            ShowMainMenu();
        }

        static void ShowMainMenu()
        {
            var mainMenuChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[red]What do you want to do[/]?")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                .AddChoices(new[] {
                    "Add new individual", "Edit individual's phone number",
                    "Search camper"
                }));

            if (mainMenuChoice == "Add new object")
            {
                var addIndividualChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]What type of object do you want to add[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                    .AddChoices(new[] {
                        "Camper", "Councelor", "NextOfKin", "Cabin"
                    }));

                if (addIndividualChoice == "Camper")
                {
                    //AddNewCamper();
                }
                else if (addIndividualChoice == "Councelor")
                {
                    Counselor newCounselor = new Counselor("undefined", "undefined", "undefined", WorkTitle.Other, DateTime.Now, DateTime.Now);
                    newCounselor.InputCounselorData();

                    AddCounselor(newCounselor);
                }
                else if (addIndividualChoice == "NextOfKin")
                {
                    NextOfKin nextOfKin = new NextOfKin("undefined", "undefined", "undefined", 0, "Father");

                    nextOfKin.InputNextOfKinData();

                    AddNextOfKin(nextOfKin);
                }
            }
            else if (mainMenuChoice == "Edit individual's phone number")
            {
                var editIndividualChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]Edit who's phone number[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                    .AddChoices(new[] {
                        "Camper", "Councelor", "NextOfKin"
                    }));

                if (editIndividualChoice == "Camper")
                {
                    //ChooseCamperToEdit();
                    //EditCamper(camper);
                }
                else if (editIndividualChoice == "Councelor")
                {
                    //ChooseCouncelorToEdit();
                    //EditCouncelor(councelor);
                }
                else if (editIndividualChoice == "NextOfKin")
                {
                    //ChooseNextOfKinToEdit();
                    //EditNextOfKin(nextOfKin);
                }
            }
            else if (mainMenuChoice == "Search camper")
            {
                var searchChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]Search camper by...[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                    .AddChoices(new[] {
                        "Councelor", "Cabin"
                    }));

                if (searchChoice == "Councelor")
                {
                    //SearchCamper(councelor);
                }
                else if (searchChoice == "Cabin")
                {
                    //SearchCamper(cabin);
                }
            }
        }

        static void AddCounselor(Counselor counselor)
        {
            using (var counselorContext = new CampContext())
            {
                counselorContext.Counselors.Add(counselor);
                counselorContext.SaveChanges();
            }
        }

        static void AddNextOfKin(NextOfKin nextOfKin)
        {
            using (var nextOfKinContext = new CampContext())
            {
                 nextOfKinContext.NextOfKins.Add(nextOfKin);
                nextOfKinContext.SaveChanges();
            }
        }

        static void AddCabin(Cabin cabin)
        {
            using (var cabinContext = new CampContext())
            {
                cabinContext.Cabins.Add(cabin);
                cabinContext.SaveChanges();
            }
        }
    }
}
