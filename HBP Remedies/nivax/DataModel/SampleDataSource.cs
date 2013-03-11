using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace PlanningDairyTemplate.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : PlanningDairyTemplate.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _createdon = string.Empty;
        public string CreatedOn
        {
            get { return this._createdon; }
            set { this.SetProperty(ref this._createdon, value); }
        }
        private string _createdtxt = string.Empty;
        public string CreatedTxt
        {
            get { return this._createdtxt; }
            set { this.SetProperty(ref this._createdtxt, value); }
        }

        private string _Colour = string.Empty;
        public string Colour
        {
            get { return this._Colour; }
            set { this.SetProperty(ref this._Colour, value); }
        }
        private string _bgColour = string.Empty;
        public string bgColour
        {
            get { return this._bgColour; }
            set { this.SetProperty(ref this._bgColour, value); }
        }
        private string _createdontwo = string.Empty;
        public string CreatedOnTwo
        {
            get { return this._createdontwo; }
            set { this.SetProperty(ref this._createdontwo, value); }
        }
        private string _createdtxttwo = string.Empty;
        public string CreatedTxtTwo
        {
            get { return this._createdtxttwo; }
            set { this.SetProperty(ref this._createdtxttwo, value); }
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get { return this._currentStatus; }
            set { this.SetProperty(ref this._currentStatus, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
           // String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "High Blood Pressure",
                    "High Blood Pressure",
                    "Assets/Images/10.jpg",
                    "High blood pressure (HBP) is a serious condition that can lead to coronary heart disease, heart failure, stroke, kidney failure, and other health problems.'Blood pressure' is the force of blood pushing against the walls of the arteries as the heart pumps blood. If this pressure rises and stays high over time, it can damage the body in many ways.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Research on HBP",
                    "The fact is, high blood pressure can lead to heart attacks, stroke, kidney failure and even blindness. But most people don't really understand what it is, or what causes it.",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\n\tOne in three adults suffers from high blood pressure, a key cause of strokes and heart disease, according to World Health Organization figures released on Wednesday. Canada and the United States have the fewest patients, at less than 20 percent of adults, but in poorer countries like Niger the estimated figure is closer to 50 percent, the UN body said. While wealthier countries have seen their cases drop thanks to effective, low-cost treatment, in Africa many remain people undiagnosed and are not getting help, according to the WHO. Its World Health Statistics report includes figures on raised blood pressure, and also on blood glucose levels, for the first time this year. One in 10 people are estimated to have diabetes, rising to up to one third in Pacific Island countries. In Niger 50.3 percent of men suffer from the condition, with Malawi and Mozambique not far behind at 44.5 and 46.3 percent respectively. The report also said obesity levels doubled across the world between 1980 and 2008 and half a billion people or 12 percent of the world's populations are now considered obese. The Americas have the highest instance, at 26 percent of adults, and south-east Asia the lowest obesity levels at three percent. The WHO said deaths in children aged under five years dropped from almost 10 million in 2000 to 7.6 million a decade later, with the decline in deaths from measles and diarrhoea-related disease particularly striking. The World Health Assembly, the decision-making body of the WHO, will meet in Geneva from May 21-26 where members will discuss new targets on cutting the cases of heart and lung disease, diabetes and cancer.",
                    group1) { CreatedOn = "Group", CreatedTxt = "High Blood Pressure", CreatedOnTwo = "Item", CreatedTxtTwo = "Research on HBP", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/11.jpg")), CurrentStatus = "HBP Remedies" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                     "Signs and symptoms",
                     "Hypertension is rarely accompanied by any symptoms, and its identification is usually through screening, or when seeking healthcare for an unrelated problem. A proportion of people with high blood pressure report headaches, as well as lightheadedness, vertigo, tinnitus.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nAt These First Signs – Go to The Doctor\n\nDon’t try to diagnosis your symptoms, you need to see your doctor.Some symptoms may be indirectly related to HBP but are not always caused by HBP, such as:\n1. Dizziness\n2. Light Headed\n3. Facial Flushing\n4. Blood Spots on the Eyes\n5. The most telling results come from blood pressure readings at high levels (systolic of 180 or higher OR diastolic of 110 or higher)\n\nSigns that accompany these levels are:\n1. Severe Headaches\n2. Severe Anxiety\n3. Shortness of Breath\n4. Nosebleeds\n\nYou should seek emergency medical treatment if you’re readings and signs are reflected in the above levels",
                     group1) { CreatedOn = "Group", CreatedTxt = "High Blood Pressure", CreatedOnTwo = "Item", CreatedTxtTwo = "Signs and symptoms", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/12.jpg")), CurrentStatus = "HBP Remedies" });
            this.AllGroups.Add(group1);

            var group2 = new SampleDataGroup("Group-2",
                   "High Blood Pressure: The Basics",
                   "High Blood Pressure: The Basics",
                   "Assets/Images/20.jpg",
                   "Your arterial system is under pressure from many elements. Medication works much faster than alternatives, the second thing is – taking to many medications can cause negative reactions – natural alternatives have a much lower risk of this happening.");
            group2.Items.Add(new SampleDataItem("Group-2-Item-1",
                    "Casuse of HBP",
                    "In almost all contemporary societies, blood pressure rises with aging and the risk of becoming hypertensive in later life is considerable. Hypertension results from a complex interaction of genes and environmental factors. Numerous common genetic variants with small effects on blood pressure have been identified",
                    "Assets/DarkGray.png",
                    "",
                    "Details:\n\n\tBlood pressure is the force blood exerts on your arterial walls. When your arterial walls constrict, blood pressure increases and this is where the problem lies. Over time, if this problem is not managed properly, it can precipitate into more serious problems. Hypertension: There is pre-hypertension and then there is hypertension. Hypertension puts pressure on your entire vascular system. Another example is sinus inflammation. Sinus pressure is a sample of the kind of stress your arterial system endures when you have high blood pressure. Your arterial system is under pressure from many elements such as toxins, processed foods, lack of exercise, stress, anxiety, and other harmful substances. It comes as no surprise that more and more younger people are having to deal with this problem. The solution is simply a matter of getting good information and then applying it.",
                    group2) { CreatedOn = "Group", CreatedTxt = "High Blood Pressure: The Basics", CreatedOnTwo = "Item", CreatedTxtTwo = "Casuse of HBP", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/21.jpg")), CurrentStatus = "HBP Remedies" });
            group2.Items.Add(new SampleDataItem("Group-2-Item-2",
                     "Natural Remedies Are Your Friends",
                     "Lifestyle changes and natural remedies may help to control high blood pressure, but your doctor may also recommend medication to lower high blood pressure. It is important to work with your doctor, because untreated high blood pressure may damage organs in the body and increase the risk of heart attack, stroke, kidney disease, and vision loss.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\n\tEveryone is different, and this is one reason the effects of natural remedies are different for each individual. This doesn’t mean that natural remedies don’t work, it means it may not work the same for everyone. Doctors will try different combination’s of drugs with patients looking for the best results, you may have to do the same when looking for a natural cure. Try one remedy and see how it works. Each of us has a slightly different system, where one person might only need 500 mgs of vitamin C you might need 3000 mgs, that’s OK. There are two differences between drugs and natural alternatives, one is the time it takes to work. Medication works much faster than alternatives, the second thing is – taking to many medications can cause negative reactions – natural alternatives have a much lower risk of this happening.",
                     group2) { CreatedOn = "Group", CreatedTxt = "High Blood Pressure: The Basics", CreatedOnTwo = "Item", CreatedTxtTwo = "Natural Remedies Are Your Friends", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/22.jpg")), CurrentStatus = "HBP Remedies" });
            group2.Items.Add(new SampleDataItem("Group-2-Item-3",
                      "Importent Note",
                      "High blood pressure is dangerous because it makes the heart work too hard. It also makes the walls of the arteries hard. High blood pressure increases the risk for heart disease and stroke, the first- and third-leading causes of death for Americans.",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\n\tAs mentioned, high blood pressure can damage arteries, and this can be fatal. Pharmacies have blood pressure devices you can use for Free. There are also blood pressure monitors for home use, and the prices are pretty reasonable too. If you experience and signs or readings out of the normal, then go see your doctor. Once you know that you do have a problem, then dig deeper into the natural remedies available.",
                      group2) { CreatedOn = "Group", CreatedTxt = "High Blood Pressure: The Basics", CreatedOnTwo = "Item", CreatedTxtTwo = "Importent Note", bgColour = "#20B2AA", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/23.jpg")), CurrentStatus = "HBP Remedies" });
            this.AllGroups.Add(group2);
			
            var group3 = new SampleDataGroup("Group-3",
                   "HBP Remidies",
                   "HBP Remidies",
                   "Assets/Images/30.jpg",
                   "It is important to take steps to keep your blood pressure under control. The treatment goal is blood pressure below 140/90 and lower for people with other conditions, such as diabetes and kidney disease. Adopting healthy lifestyle habits is an effective first step in both preventing and controlling high blood pressure.");
            group3.Items.Add(new SampleDataItem("Group-3-Item-1",
                    "Diet",
                    "There’s a High Blood Pressure Diet that’s just right for you. A diet should be tailored to the specific needs of the patient, a diet that will help regulate and lower your blood pressure risks. Yes, with elevated blood pressure you have risks and these can be fatal if you ignore the signs.",
                    "Assets/DarkGray.png",
                    "",
                    "Details:\n\n\tThere is no reason NOT to check your blood pressure these days. Visiting your Doctor is one way to get your blood pressure checked – even your local pharmacy has a portable unit that will do a quick check – most times for Free. You also have the option to purchase a portable home device, in some cases your insurance company will pay for it. When you find the right high blood pressure diet you’ll want to check your stats out more often. This will be a way to tell if the diet is working or not. If the new diet is working…stick with it. As we get older diet becomes more and more important, changing the type of foods to lower blood pressure will be the challenge. Going back to old habits will only lead to more problems – problems you are trying to solve.\n\nLets take a look at some High Blood Pressure Diets\n\nIt’s almost impossible to eat 100% natural. We’ll try to choose as many natural foods to lower blood pressure focusing on the ones that are readily available at your local stores. Lets start…\n\n1.  Whole Grain – A box of natural oats will last months and cost maybe $7.00. Sweeten with honey. The main benefit is for cholesterol, but this affects your blood pressure and is a great source of natural energy.\n\n2.  Fruits and Vegetables – Easily converted sugar for energy, good source of vitamins and minerals, some fiber for cleaning your colon and is easy on your digestive system.\n\n3.  Beef Alternative – Ostrich or Buffalo. As high blood pressure increases in our society the reemergence of Ostrich and Buffalo meats could make a comeback. The primary market demand is much lower fat and 1/3 the calories of chicken. It’s not common, but I hear rumblings that maybe there’s a demand for these meats on the horizon.\n\n4.  Cooking Spices – Adding these spices such as fennel, oregano, black pepper, basil and tarragon to your cooking routine is beneficial in high blood pressure. Use them in your cooking.\n\n5.  Onion – Are useful in hypertension. What is best is the onion essential oil. Two to three tablespoons of onion essential oil a day was found to lower the systolic levels by an average of 25 points and the diastolic levels by 15 points in hypertension subjects. This should not be surprising because onion is a cousin of garlic.\n\n6. Tomatoes – Are high in gamma-amino butyric acid (GABA), a compound that can help bring down blood pressure.\n\n7. Broccoli – This vegetable contains several active ingredients that reduce blood pressure.\n\nThe most beneficial food you can add to your diet is raw vegetables. This may not sit well with some people so the next best thing are steamed vegetables. It’s also claimed that the juice from boiling vegetables is more nutritious that the veggies themselves. If you combine some of the remedies found on this site with a proper diet, then you are on your way to a fantastic start to putting together a plan for lowering your blood pressure. You can’t go wrong with what nature has provided for you naturally, but remember: It’s always advisable to check with your Doctor before you make any changes to your existing routine. Many times medication is needed to rectify immediate problems, but long term we know there can be negative side effects – this is documented. It does no harm to look at natural ways to manage your blood pressure for the long term. We have provided several ideas on how to reduce your blood pressure naturally – yoga, meditation, teas – take a look for yourself and discover how you can manage this condition using alternative methods.",
                    group3) { CreatedOn = "Group", CreatedTxt = "HBP Remidies", CreatedOnTwo = "Item", CreatedTxtTwo = "Diet", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/31.jpg")), CurrentStatus = "HBP Remedies" });
            group3.Items.Add(new SampleDataItem("Group-3-Item-2",
                     "Meditation",
                     "Surprise, surprise, reducing stress helps in reducing high blood pressure. The question is – How to reduce stress? One answer we found to be effective is the ancient practice of meditation – meditation for high blood pressure. Some of the modern meditation techniques have been refined to accommodate today’s busy schedules.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\n\tMeditation is a technique used to train your mind, focus on relaxation and take the focus away from external pressures that cause stress and high blood pressure. Stress causes tightness and tension, this constricts arteries and causes your blood pressure to rise. There have been more than 600 studies over the past 30 years using meditation for high blood pressure problems. The average reduction in blood pressure was about 8 points. Squeezing Meditation Into Your Busy Day Some meditation techniques are time consuming, but others will take less than 10 minuets to do. Transcendental meditation (TM) is the only form that’s proven effective in reducing the risks of heart conditions. Researchers believe that the deep rest achieved through TM sparks biochemical changes that help the body and mind reach a more balanced state, in turn triggering the body’s own self-repair mechanism. Most TM practitioners suggest 2 sessions a day for 20 minuets is the most effective schedule, but in a moment you will discover techniques you can do at the office in half the time. Meditation has other benefits besides managing high blood pressure, meditation improves awareness and alertness. The goal of meditation is to  remove harmful negative thoughts and replace them with positive ones, this can really change lives.",
                     group3) { CreatedOn = "Group", CreatedTxt = "HBP Remidies", CreatedOnTwo = "Item", CreatedTxtTwo = "Meditation", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/32.jpg")), CurrentStatus = "HBP Remedies" });
            group3.Items.Add(new SampleDataItem("Group-3-Item-3",
                      "Exercise",
                      "A close cousin to meditation is yoga.",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\n\tYoga is a very low impact exercise routine that is used for relaxation. The key to both yoga and meditation is balancing mind and body for a peaceful existence. You can read more about this at: controlling high blood pressure using yoga.\n\nKeep These Things In Mind\nIt’s suggested you do these meditation techniques early in the morning and later in the evening. These techniques are effective because you are doing two things:\n\n1.  You’re eliminating stress.\n2.  You are allowing your body to release it’s natural healing powers.\n\nThe combination of these two things is very effective in lowering blood pressure and contributes to better overall health.You can now add what you have learned to your blood pressure remedy kit.",
                      group3) { CreatedOn = "Group", CreatedTxt = "HBP Remidies", CreatedOnTwo = "Item", CreatedTxtTwo = "Exercise", bgColour = "#20B2AA", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/33.jpg")), CurrentStatus = "HBP Remedies" });
            group3.Items.Add(new SampleDataItem("Group-3-Item-4",
                      "Foods",
                      "Food – It’s a shame that foods which taste good are the same ones that cause the most problems. Managing your diet is one way you can lower your blood pressure. We use the word manage in place of “change,” because it sounds like an easier task to do, but lets not kid ourselves – changing habits is hard…really hard.",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\n\tOne way you can change your diet is – don’t buy foods that aggravate your blood pressure. I will buy a half-gallon of ice cream (caramel swirl type). All it takes is that first taste, and the container is empty in a day or two. If I don’t buy it, this won’t happen, but it’s a torturous way to live. So what else can we try?\n\nSubstitutions: There are plenty of food substitutes out there you can try. Reduced salt, low fat – this is one place to start.\n\nOne Change at a Time: Trying to change to many things at once just doesn’t work. Not only are you fighting habits, but your body chemistry needs time to adjust. Changing one part of your diet at a time gives your mind and body the chance to adjust. What we’re looking for is a permanent change.\n\nThe Big Time Obstacle – Mindset: We should call this “managing your mind,” because your mind is the center of all habits and emotions. There are very few people who can just turn the switch off and change their mind on a dime, and really do it long-term. One method to look at is meditation. Your mind is designed to retain and protect information – good or bad – so bad habits are protected the same as good ones. Meditation is one method used to uproot this bad programming and replace it with a new habit – it works.\n\nFoods to Lower Blood Pressure\n\nBananas: High in potassium and fiber, the banana is convenient, tasty, inexpensive, and with a little imagination can make a sinfully delicious dessert.\n\nTomatoes: Another source of potassium and also of calcium. The tomato has a powerful antioxidant called lycopene that can also help prevent heart disease. Lycopene prevent LDL cholesterol from oxidizing and sticking to the blood vessels. Fresh is best. If you get your tomato through juice, make sure it’s pure and does not contain additives and salt. If you’re into pasta, try making your own sauce. It can be fun and the house will smell wonderful as you are making it.\n\nBlueberries and Blackberries: Both are packed with powerful antioxidants and are another source of dietary fiber.\n\nGarlic: Garlic is one of those miracle foods that has been used medicinally for over a thousand years. It has the capacity to thin the blood thereby making it easier for the heart to pump. It has also been suggested that garlic is a defense against certain digestive cancers including colon cancer.\n\nCold Water Fish: Salmon, sardines, halibut, tuna and mackerel are great sources of Omega-3. Try to replace a red meat meal two or three time per week with one of these fish.\n\nDark Chocolate and Cocoa:  Yes there is some fat and sugar in there but there is also some very powerful antioxidants. More importantly, if you are going to change your diet you still need to have some kind of reward food for sticking to it. Combine that dark chocolate with baked bananas and you have a very tasty dessert.\n\nWater: Okay you may not think of water as a food but it is essential not only to your blood pressure but to your entire body.  Dehydration is becoming a bigger health issue than it was before. As a rule, you should drink half your body weight in ounces daily. So if you weigh 150 pounds, you should drink 75 ounces of water or about 4 standard bottles a day.\n\nExtra-Virgin Olive Oil: High in oleic acid and polyphenols, an antioxidant.\nTips:\n\nMicrowave: Try not to microwave to many foods. When you’re working on a diet change and switching to natural foods, you want to preserve as much nutrition in the food as possible. Microwaving reduces the nutrients from food.\n\nEating Out: When going to restaurants request less salt and sauces that can aggravate your blood pressure. This way you can enjoy dining out and still stay on the path to better health.\n\nStress: Staying away from situations that creates stress for you is another way to keep your blood pressure down. Stress can drive your blood pressure through the roof, find ways to reduce stress.\n\nAs mentioned, if you try to change to much to fast, the chances are good that you will fail. Consider using meditation as a tool to enforce change. We started you with a list of foods to lower blood pressure, but there are plenty of other foods you can eat that will help you manage your blood pressure – food is one of the best natural remedies for treating blood pressure.",
                      group3) { CreatedOn = "Group", CreatedTxt = "HBP Remidies", CreatedOnTwo = "Item", CreatedTxtTwo = "Foods", bgColour = "#808000", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/34.jpg")), CurrentStatus = "HBP Remedies" });
            group3.Items.Add(new SampleDataItem("Group-3-Item-5",
                     "Supplements",
                     "Your body needs vitamins, and certain vitamins can help manage high blood pressure. The one thing people fail to realize is your body needs time to metabolize vitamins, and people are not always patient, so they give up to soon on supplements. There’s one other factor to discuss – it’s the subject of absorption.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\n\tDigestion and Absorption are two words misunderstood. The bottom line is if you can’t digest and absorb anything properly, then you can never get the maximum benefits – it’s that simple. Studies show that more than 30 percent of what we ingest never gets into our system. It passes right through and ends up in the toilet – this is where the money you paid for it goes too. This is one reason people feel that natural remedies don’t work; they overlook the digestion\absorption factor. Remember the other thing we discussed? Vitamins and supplements take longer to work.\n\nVitamins That Effect High Blood Pressure:\n\nLets take a look at what effect vitamins and minerals have on high blood pressure.\n\nVitamin A – is a fat-soluble vitamin that, like vitamin E, works primarily in the lipid, or fat, tissues of your body. Vitamin A prevents LDL cholesterol (bad cholesterol) building up of plague in your arteries.\n\nVitamin C – protects your arteries and decreases the risk of heart disease and hypertension. It prevents the generation of free radicals which damage artery walls. It helps repair damage to the arteries, preventing the deposit of plaque at the site of injury. It also helps elevate levels of HDL cholesterol (good cholesterol).\n\nVitamin E – is the most active antioxidant in the fatty cells of your body. It reduces the risk of heart disease and hypertension by protecting your arteries from the effects of oxidized LDL cholesterol.\n\nPotassium – even though it’s not a vitamin I include it because it’s easy to get and has an affect on high blood pressure. Bananas, potatoes and milk are good sources for getting potassium.\n\nMagnesium – again, is not a vitamin but does benefit you and is a supplement easily found. People require between 300 and 400 milligrams of magnesium each day. Salmon, leafy green vegetables, whole grains and nuts are good sources of magnesium.\n\nNow that we’ve explained some of the benefits vitamins have, we need to look at quality. The quality of a vitamin is just as important as its benefits for high blood pressure.",
                     group3) { CreatedOn = "Group", CreatedTxt = "HBP Remidies", CreatedOnTwo = "Item", CreatedTxtTwo = "Supplements", bgColour = "#2E8B57", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/35.jpg")), CurrentStatus = "HBP Remedies" });
            this.AllGroups.Add(group3);
			
            var group4 = new SampleDataGroup("Group-4",
                   "Herbal Tea to Lower BP",
                   "Herbal Tea to Lower BP",
                   "Assets/Images/40.jpg",
                   "Lowering high blood pressure is as easy as one, two, tea: Study participants who sipped 3 cups of a hibiscus tea daily lowered systolic blood pressure by 7 points in 6 weeks on average, say researchers from Tufts University—results on par with many prescription medications. Those who received a placebo drink improved their reading by only 1 point.");
            group4.Items.Add(new SampleDataItem("Group-4-Item-1",
                      "Black",
                      "The largest producers of black tea are India and China. Why is black tea good for lowering blood pressure? It all revolves around a substance called Catechins. Catechins is a substance in black tea that is an antioxidant – it fights free radicals in your body. There are other benefits too, there is a compound found in black tea called quercetin.",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\n\tCatechins is a substance in black tea that is an antioxidant – it fights free radicals in your body. There are other benefits too, there is a compound found in black tea called quercetin. Quercetin is a phytochemical that helps reduce the risk of heart attacks and cancer. The other compound discovered in this tea that reduces the risk of cancer is called TF-2. This compound causes cancer cells to go into a so called “Death Cycle”. Another term for this is apoptosis. Amazingly only the cancer cells are affected and not normal cells. People who smoke cigarettes and other tobacco  products should pay close attention to this claim.",
                      group4) { CreatedOn = "Group", CreatedTxt = "Herbal Tea to Lower BP", CreatedOnTwo = "Item", CreatedTxtTwo = "Black", bgColour = "#20B2AA", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/41.jpg")), CurrentStatus = "HBP Remedies" });
            group4.Items.Add(new SampleDataItem("Group-4-Item-2",
                      "Green",
                      "There are hundreds of green tea blends, but there is one substance that most green teas have in common and its name is catechin.",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\n\tCatechin inhibits the production of angiotensin II which helps in lowering blood pressure. In simpler terms – green tea lowers blood pressure by lowering the risk of cardiovascular disease. The flavonoid components of tea have been associated in epidemiological studies with a decreased risk of cardiovascular disease. Flavonoids have been shown to have antioxidant and vasodilator effects in vitro; we therefore postulated that drinking green or black tea attenuates the well-characterized acute pressor response to caffeine and lowers blood pressure during regular consumption.",
                      group4) { CreatedOn = "Group", CreatedTxt = "Herbal Tea to Lower BP", CreatedOnTwo = "Item", CreatedTxtTwo = "Green", bgColour = "#808000", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/42.jpg")), CurrentStatus = "HBP Remedies" });
            group4.Items.Add(new SampleDataItem("Group-4-Item-3",
                     "Habicius",
                     "People with high blood pressure (hypertension) can lower their blood pressure by drinking a tea made from a standardized extract of hibiscus flower every day.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\n\tHibiscus tea is one of those teas that date back thousands of years. The Ancient Egypt Pharaohs indulged themselves with this drink to counter the Nile Valley heat.  Hibiscus tea has antioxidant properties, and, in animal models, extracts of its calyces have demonstrated hypocholesterolemic and antihypertensive properties.",
                     group4) { CreatedOn = "Group", CreatedTxt = "Herbal Tea to Lower BP", CreatedOnTwo = "Item", CreatedTxtTwo = "Habicius", bgColour = "#2E8B57", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/43.jpg")), CurrentStatus = "HBP Remedies" });
            this.AllGroups.Add(group4);
        }
    }
}
