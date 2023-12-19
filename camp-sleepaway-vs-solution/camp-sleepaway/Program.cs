using Spectre.Console;

namespace camp_sleepaway
{
    public class Program
    {
        // Samuel Lööf, Simon Sörqvist, Adam Kumlin
        public static void Main()
        {
            ShowMainMenu();
        }

        internal static void ShowMainMenu()
        {
            var mainMenuChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[red]What do you want to do[/]?")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                .AddChoices(new[] {
                    "Add new individual", "Edit individual",
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
                    Camper camper = Camper.InputCamperData();

                    camper.SaveToDb();
                }
                else if (addIndividualChoice == "Councelor")
                {
                    Counselor counselor = Counselor.InputCounselorData();

                    counselor.SaveToDb();
                }
                else if (addIndividualChoice == "NextOfKin")
                {
                    NextOfKin nextOfKin = NextOfKin.InputNextOfKinData();

                    nextOfKin.SaveToDb();
                }
                else if (addIndividualChoice == "Cabin")
                {
                    Cabin cabin = Cabin.InputCabinData();

                    cabin.SaveToDb();
                }
            }
            else if (mainMenuChoice == "Edit individual")
            {
                var editIndividualChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]Who do you wish to edit? [/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                    .AddChoices(new[] {
                        "Camper", "Councelor", "NextOfKin"
                    }));

                if (editIndividualChoice == "Camper")
                {
                    Camper camper = Camper.ChooseCamperToEdit();
                    if (camper != null)
                    {
                        Camper editedCamper = Camper.EditCamperMenu(camper);
                    }
                    else
                    {
                        Console.WriteLine("No camper has been selected for editing. ");
                    }

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

        
    }
}
