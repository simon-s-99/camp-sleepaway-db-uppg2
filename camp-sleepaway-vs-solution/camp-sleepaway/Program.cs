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

            if (mainMenuChoice == "Add new individual")
            {
                var addIndividualChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]What type of individual do you want to add[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                    .AddChoices(new[] {
                        "Camper", "Councelor", "NextOfKin"
                    }));

                if (addIndividualChoice == "Camper")
                {
                    //AddNewCamper();
                }
                else if (addIndividualChoice == "Councelor")
                {
                    //AddNewCouncelor();
                }
                else if (addIndividualChoice == "NextOfKin")
                {
                    //AddNewNextOfKin();
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

    }
}
