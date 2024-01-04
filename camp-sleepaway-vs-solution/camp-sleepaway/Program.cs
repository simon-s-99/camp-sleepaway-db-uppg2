// Do not remove using camp_sleepaway.test_data_for_tables;
// namespace was adjusted to avoid unintended calls from other
// places in the program and to simplify intellisense suggestions.
using camp_sleepaway.test_data_for_tables;
using Spectre.Console;

// Samuel Lööf, Simon Sörqvist, Adam Kumlin

namespace camp_sleepaway
{
    public class Program
    {
        public static void Main()
        {
            /*
             * ATTENTION! ::
             *                  Run command "Update-Database" 
             *                  in Package Manager Console before
             *                  attempting to run program. 
            */

            // Checks if this is the programs first execution, adds example data if true
            IsFirstRun();
            Console.WriteLine();

            ShowMainMenu();
        }

        internal static void IsFirstRun()
        {
            // checks for a .txt file with one line ending with a "1"-character
            string dir = Directory.GetCurrentDirectory() + "\\first_run.txt";
            string rawText = File.ReadAllText(dir);
            bool firstRun = rawText.EndsWith('1');

            if (firstRun)
            {
                Console.WriteLine("Adding first time data.");
                AddExampleDataToDb.AddAllData();

                // this only gets replaced in debug/build file, i.e. no visible change in visual studio
                string newText = rawText.Replace('1', '0');
                File.WriteAllText(dir, newText);
            }
            else
            {
                Console.WriteLine("First time data already exists.");
            }
        }

        internal static void ShowMainMenu()
        {
            bool firstTimeDrawMenuOrGoBack = true;

            while (true)
            {
                // so that we do not prompt user to press any key first time main menu is shown 
                if (!firstTimeDrawMenuOrGoBack)
                {
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                }

                firstTimeDrawMenuOrGoBack = false;

                string[] mainMenuChoiceOptions = { "Add new object", "Edit object", "Search camper",
                "View campers and NextOfKins", "Delete object", "Exit program" };

                string mainMenuChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[red]What do you want to do[/]?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                        .AddChoices(mainMenuChoiceOptions));

                Console.Clear();

                // Add new object 
                if (mainMenuChoice == mainMenuChoiceOptions[0])
                {
                    string[] addIndividualChoiceOptions =
                    {
                        "Camper", "Counselor", "NextOfKin", "Cabin", "Go back"
                    };
                    string? addIndividualChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[red]What type of object do you want to add[/]?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                        .AddChoices(addIndividualChoiceOptions));

                    Console.Clear();

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

                        if (counselor.CabinId != null && counselor.CabinId != 0)
                        {
                            Cabin counselorCabin = Counselor.UpdateCabinWithCounselorId(counselor.CabinId, counselor);
                            counselorCabin.UpdateRecordInDb();
                        }
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

                        if (cabin.CounselorId != null)
                        {
                            Counselor cabinCounselor = Cabin.UpdateCounselorWithCabinId(cabin.CounselorId, cabin);
                            cabinCounselor.UpdateRecordInDb();
                        }
                    }
                    // go back menu choice
                    else
                    {
                        // so that user does not have to press any key if they go back
                        firstTimeDrawMenuOrGoBack = true;
                    }
                }
                // Edit record in db 
                else if (mainMenuChoice == mainMenuChoiceOptions[1])
                {
                    Console.Clear();

                    string[] editIndividualChoiceOptions = { "Camper", "Counselor", "NextOfKin", "Cabin", "Go back" };
                    string? editIndividualChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[red]What do you wish to edit[/]?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                        .AddChoices(editIndividualChoiceOptions));

                    Console.Clear();

                    // Camper
                    if (editIndividualChoice == editIndividualChoiceOptions[0])
                    {
                        Camper camper = Camper.ChooseCamperMenu();
                        if (camper != null)
                        {
                            Camper editedCamper = Camper.EditCamperMenu(camper, null);
                            editedCamper.UpdateRecordInDb();
                        }
                        else
                        {
                            Console.WriteLine("No camper has been selected for editing. ");
                        }

                    }
                    // Counselor
                    else if (editIndividualChoice == editIndividualChoiceOptions[1])
                    {
                        Counselor counselor = Counselor.ChooseCounselorMenu();
                        if (counselor != null)
                        {
                            Counselor editedCounselor = Counselor.EditCounselorMenu(counselor);
                            editedCounselor.UpdateRecordInDb();

                            if (counselor.CabinId != null && counselor.CabinId != 0)
                            {
                                Cabin counselorCabin = Counselor.UpdateCabinWithCounselorId(editedCounselor.CabinId, editedCounselor);
                                counselorCabin.UpdateRecordInDb();
                            }
                        }
                        else
                        {
                            Console.WriteLine("No counselor has been selected for editing. ");
                        }
                    }
                    // NextOfKin
                    else if (editIndividualChoice == editIndividualChoiceOptions[2])
                    {
                        NextOfKin nextOfKin = NextOfKin.ChooseNextOfKinMenu();
                        if (nextOfKin != null)
                        {
                            NextOfKin editedNextOfKin = NextOfKin.EditNextOfKinMenu(nextOfKin);
                            editedNextOfKin.UpdateRecordInDb();
                        }
                        else
                        {
                            Console.WriteLine("No NextOfKin has been selected for editing. ");
                        }
                    }
                    // Cabin
                    else if (editIndividualChoice == editIndividualChoiceOptions[3])
                    {
                        Cabin cabin = Cabin.ChooseCabinMenu();
                        if (cabin != null)
                        {
                            Cabin editedCabin = Cabin.EditCabinMenu(cabin);
                            editedCabin.UpdateRecordInDb();
                        }
                        else
                        {
                            Console.WriteLine("No cabin has been selected for editing. ");
                        }
                    }
                    // go back menu choice
                    else
                    {
                        // so that user does not have to press any key if they go back
                        firstTimeDrawMenuOrGoBack = true;
                    }
                }
                // Search camper
                else if (mainMenuChoice == mainMenuChoiceOptions[2])
                {
                    Camper.SearchCamper();
                }
                // Display campers and NextOfKins
                else if (mainMenuChoice == mainMenuChoiceOptions[3])
                {
                    Camper.DisplayCampersAndNextOfKins();
                }
                // Delete row 
                else if (mainMenuChoice == mainMenuChoiceOptions[4])
                {
                    Console.Clear();

                    string[] deleteObjectChoiceOptions = { "Camper", "Counselor", "NextOfKin", "Cabin", "Go back" };
                    string? deleteObjectChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[red]Which table do you wish to delete from[/]?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                        .AddChoices(deleteObjectChoiceOptions));

                    Console.Clear();

                    // Camper
                    if (deleteObjectChoice == deleteObjectChoiceOptions[0])
                    {
                        Camper camper = Camper.ChooseCamperMenu();

                        // remove camper from it's cabin
                        Cabin cabin = Camper.GetCabinFromCabinId(camper.CabinId);
                        if (cabin != null)
                        {
                            cabin.Campers.Remove(camper);
                            cabin.UpdateRecordInDb();
                        }

                        // remove campers associated NextOfKins
                        NextOfKin[] nextOfKins = Camper.GetNextOfKinsFromCamperID(camper.Id);
                        foreach (NextOfKin nextOfKin in nextOfKins)
                        {
                            nextOfKin.DeleteFromDb();
                        }

                        camper.DeleteFromDb();
                    }
                    // Counselor
                    else if (deleteObjectChoice == deleteObjectChoiceOptions[1])
                    {
                        Counselor[] existingCounselors = Counselor.GetAllFromDb();
                        Camper[] existingCampers = Camper.GetAllFromDb();

                        // use ceiling to always round up, if we have 13 campers we should not delete
                        // counselor nr. 4, this would throw an error, ceiling fixes this 
                        if (existingCounselors.Length <= Math.Ceiling((double)existingCampers.Length / 4))
                        {
                            Console.WriteLine("Each counselor is responsible for 1-4 campers " +
                                "you can not remove a counselor if their campers have not yet left.");
                        }
                        else
                        {
                            Counselor counselor = Counselor.ChooseCounselorMenu();
                            Cabin counselorCabin = Camper.GetCabinFromCabinId(counselor.CabinId);
                            counselorCabin.CounselorId = null;
                            counselorCabin.Counselor = null;
                            counselorCabin.UpdateRecordInDb();
                            counselor.DeleteFromDb();
                        }
                    }
                    // NextOfKin
                    else if (deleteObjectChoice == deleteObjectChoiceOptions[2])
                    {
                        NextOfKin nextOfKin = NextOfKin.ChooseNextOfKinMenu();
                        nextOfKin.DeleteFromDb();
                    }
                    // Cabin 
                    else if (deleteObjectChoice == deleteObjectChoiceOptions[3])
                    {
                        Cabin[] existingCabins = Cabin.GetAllFromDb();
                        Camper[] existingCampers = Camper.GetAllFromDb();

                        if (existingCabins.Length <= Math.Ceiling((double)existingCampers.Length / 4))
                        {
                            Console.WriteLine("Each cabin houses 1-4 campers, you can not remove a cabin " +
                                "if it's campers have not yet left or been re-assigned.");
                        }
                        else
                        {
                            Cabin cabin = Cabin.ChooseCabinMenu();

                            if (cabin.CounselorId != null)
                            {
                                Counselor cabinCounselor = Cabin.GetCounselorFromCabinId(cabin.Id);
                                cabinCounselor.CabinId = null;
                                cabinCounselor.Cabin = null;
                                cabinCounselor.UpdateRecordInDb();
                            }

                            Camper[] cabinCampers = Cabin.GetCampersFromCabinId(cabin.Id);
                            foreach (Camper camper in cabinCampers)
                            {
                                camper.CabinId = null;
                                camper.UpdateRecordInDb();
                            }

                            cabin.DeleteFromDb();
                        }

                    }
                    // go back menu choice
                    else
                    {
                        // so that user does not have to press any key if they go back
                        firstTimeDrawMenuOrGoBack = true;
                    }
                }
                // exit program
                else
                {
                    Console.Clear();
                    Console.WriteLine("Exiting program, goodbye.");
                    Console.WriteLine();
                    break;
                }
            }
        } // <-- end of ShowMainMenu() method
    }
}
