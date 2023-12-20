using camp_sleepaway.ef_table_classes;
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
            string[] mainMenuChoiceOptions = { "Add new object", "Edit individual", "Search camper" };
            string? mainMenuChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[red]What do you want to do[/]?")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                .AddChoices(mainMenuChoiceOptions));

            // Add new object 
            if (mainMenuChoice == mainMenuChoiceOptions[0])
            {
                string[] addIndividualChoiceOptions = { "Camper", "Counselor", "NextOfKin", "Cabin" };
                string? addIndividualChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]What type of object do you want to add[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                    .AddChoices(addIndividualChoiceOptions));

                // Camper
                if (addIndividualChoice == addIndividualChoiceOptions[0])
                {
                    Camper camper = Camper.InputCamperData();

                    camper.SaveToDb();
                }
                // Counselor
                else if (addIndividualChoice == addIndividualChoiceOptions[1])
                {
                    Counselor counselor = Counselor.InputCounselorData();

                    counselor.SaveToDb();
                }
                // NextOfKin
                else if (addIndividualChoice == addIndividualChoiceOptions[2])
                {
                    NextOfKin nextOfKin = NextOfKin.InputNextOfKinData();

                    nextOfKin.SaveToDb();
                }
                // Cabin 
                else if (addIndividualChoice == addIndividualChoiceOptions[3])
                {
                    Cabin cabin = Cabin.InputCabinData();

                    cabin.SaveToDb();
                }
            }
            // Edit individual
            else if (mainMenuChoice == mainMenuChoiceOptions[1])
            {
                string[] editIndividualChoiceOptions = { "Camper", "Counselor", "NextOfKin" };
                string? editIndividualChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]Who do you wish to edit? [/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                    .AddChoices(editIndividualChoiceOptions));

                // Camper
                if (editIndividualChoice == editIndividualChoiceOptions[0])
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
                // Counselor
                else if (editIndividualChoice == editIndividualChoiceOptions[1])
                {
                    Counselor counselor = Counselor.ChooseCounselorToEdit();
                    if (counselor != null)
                    {
                        Counselor editedCounselor = Counselor.EditCounselorMenu(counselor);
                    }
                    else
                    {
                        Console.WriteLine("No counselor has been selected for editing. ");
                    }
                }
                // NextOfKin
                else if (editIndividualChoice == editIndividualChoiceOptions[2])
                {
                    NextOfKin nextOfKin = NextOfKin.ChooseNextOfKinToEdit();
                    if (nextOfKin != null)
                    {
                        NextOfKin editedNextOfKin = NextOfKin.EditNextOfKinMenu(nextOfKin);
                    }
                    else
                    {
                        Console.WriteLine("No NextOfKin has been selected for editing. ");
                    }
                }
            }
            // Search camper
            else if (mainMenuChoice == mainMenuChoiceOptions[2])
            {
                Camper.SearchCamper();
            }
        }

        
    }
}
