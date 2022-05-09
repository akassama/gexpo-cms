/* ACCOUNTS */
DROP TABLE IF EXISTS Accounts                                                                                                                                                                                                                                                                                                           /* ACCOUNTS */
CREATE TABLE [dbo].[Accounts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [varchar](255) NOT NULL,
	[FirstName] [varchar](255) NULL,
	[LastName] [varchar](255) NULL,
	[Email] [varchar](255) NOT NULL,
	[Description] [text] NULL,
	[Gender] [varchar](50) NULL,
	[Country] [varchar](50) NULL,
	[Address] [varchar](255) NULL,
	[DateOfBirth] [varchar](255) NULL,
	[PhoneNumber] [varchar](255) NULL,
	[Password] [varchar](500) NOT NULL,
	[ProfilePicture] [varchar](255) NULL,
	[Active] [bit] DEFAULT 0,
	[Oauth] [bit] DEFAULT 0,
	[EmailVerification] [bit] DEFAULT 0,
	[EmailNotification] [bit] DEFAULT 0,
	[DirectoryName] [varchar](255) NOT NULL,
	[SessionKey] [varchar](255) NOT NULL,
	[RememberToken] [varchar](500) NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[Accounts] ADD CONSTRAINT unique_account_email UNIQUE (Email);
GO
ALTER TABLE [dbo].[Accounts] ADD CONSTRAINT unique_account_id UNIQUE (AccountID);
GO

INSERT INTO [Accounts]
([AccountID], [FirstName], [LastName], [Email], [Password], [ProfilePicture], [Active], [DirectoryName], [SessionKey], [UpdatedAt])
VALUES
('41924010-149f-4f07-9821-b3998ffaebba', 'Admin', 'User', 'admin@example.com', '$2b$10$QNoy2uVBJIhNMYws81r6G.ZI5mp0ChEYe6YcHsf1nL3tcR7et13D2', 'admin-profile-picture.jpg', 'True', 'adminet2zxn5m', 'fa042c51-4c66-4a13-b175-133815a6df8c', GETDATE());




/* PERMISSIONS */
DROP TABLE IF EXISTS Permissions 
CREATE TABLE [dbo].[Permissions](
	[PermissionID] [int] IDENTITY(1,1) NOT NULL,
	[PermissionName] [varchar](250) NOT NULL,
	[ShortDescription] [varchar](500) NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[Permissions]
ADD CONSTRAINT unique_permission_name UNIQUE (PermissionName);
GO

INSERT INTO [Permissions]
([PermissionName], [ShortDescription], [UpdatedAt])
VALUES
('Author Permissions', 'User can create, edit, and manage posts and pages', GETDATE()),
('Editor Permissions', 'User can perform author actions and manage website data', GETDATE()),
('Admin Permissions', 'User can perform editor actions, manage website configurations and users', GETDATE());




DROP TABLE IF EXISTS AccountToPermission 
CREATE TABLE [dbo].[AccountToPermission](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [varchar](250) NOT NULL,
	[PermissionID] [int] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate()),
	PRIMARY KEY (AccountID, PermissionID)
)
ALTER TABLE [dbo].[AccountToPermission]
ADD CONSTRAINT unique_account_to_permission UNIQUE ([AccountID], [PermissionID]);
GO

INSERT INTO [AccountToPermission]
([AccountID], [PermissionID], [UpdatedAt])
VALUES
('41924010-149f-4f07-9821-b3998ffaebba', 1, GETDATE()),
('41924010-149f-4f07-9821-b3998ffaebba', 2, GETDATE()),
('41924010-149f-4f07-9821-b3998ffaebba', 3, GETDATE());
                                                                                                                                                                   

/* Configurations */
DROP TABLE IF EXISTS Configurations 
CREATE TABLE [dbo].[Configurations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ConfigurationID] [varchar](255) NOT NULL,
	[ConfigurationKey] [varchar](255) NOT NULL,
	[ConfigurationValue] [varchar](2500) NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[Configurations] ADD CONSTRAINT unique_config_id UNIQUE (ConfigurationID);
GO
ALTER TABLE [dbo].[Configurations] ADD CONSTRAINT unique_config_key UNIQUE (ConfigurationKey);
GO

INSERT INTO [Configurations]
([ConfigurationID], [ConfigurationKey], [ConfigurationValue], [UpdatedAt])
VALUES
(NEWID(), 'GoogleAnalyticsKey', 'UA-XXXXXXXXXX-X', GETDATE()),
(NEWID(), 'GoogleAdvertKey', 'ca-pub-XXXXXXXXXXXXXX', GETDATE()),
(NEWID(), 'ShowFacebookComments', '1', GETDATE()),
(NEWID(), 'FacebookCommentId', '200XXXXXXXXXXXXXX', GETDATE()),
(NEWID(), 'ShowTwitterFeeds', '1', GETDATE()),
(NEWID(), 'TwitterFeed', '<a class="twitter-timeline" href="https://twitter.com/WHO?ref_src=twsrc%5Etfw">Tweets by WHO</a> <script async src="https://platform.twitter.com/widgets.js" charset="utf-8"></script>', GETDATE()),
(NEWID(), 'SmtpEmail', 'your-email@mail.com', GETDATE()),
(NEWID(), 'SmtpPassword', 'Password', GETDATE()),
(NEWID(), 'SmtpPort', '547', GETDATE()),
(NEWID(), 'SmtpHost', 'smtp.mail.com', GETDATE()),
(NEWID(), 'SenderEmail', 'your-sender-email@mail.com', GETDATE()),
(NEWID(), 'EmailDisplayName', 'Your Email Display Name', GETDATE()),
(NEWID(), 'EmailClosureText', 'Your Email Closure Text', GETDATE()),
(NEWID(), 'EmailerType', 'Default', GETDATE()),
(NEWID(), 'SendGridKey', '', GETDATE()),
(NEWID(), 'ShowCookieConcent', '1', GETDATE()),
(NEWID(), 'CookieConcent', '<!-- Cookie Consent by https://www.FreePrivacyPolicy.com --> <script type="text/javascript" src="//www.freeprivacypolicy.com/public/cookie-consent/3.1.0/cookie-consent.js"></script> <script type="text/javascript"> document.addEventListener(’DOMContentLoaded’, function () { cookieconsent.run({"notice_banner_type":"headline","consent_type":"express","palette":"light","language":"en","website_name":"YOUR WEBSITE NAME"}); }); </script>', GETDATE()),
(NEWID(), 'EnableJavaScripts', '1', GETDATE()),
(NEWID(), 'HeaderJavaScripts', '', GETDATE()),
(NEWID(), 'FooterJavaScripts', '', GETDATE()),
(NEWID(), 'ShowShareThis', '1', GETDATE()),
(NEWID(), 'ShareThisUrl', 'https://platform-api.sharethis.com/js/sharethis.js#property=5fXXXXXXXXXXXXXXXXXXXXXXX&amp;product=sop', GETDATE());

                                                                                            

/* Posts */
DROP TABLE IF EXISTS Posts 
CREATE TABLE [dbo].[Posts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PostID] [varchar](255) NOT NULL,
	[PostType] [varchar](100) NOT NULL,
	[Slug] [varchar](255) NOT NULL,
	[Author] [varchar](255) NOT NULL,
	[Categories] [varchar](255) NULL,
	[Title] [varchar](255) NOT NULL,
	[ShortDescription] [varchar](500) NULL,
	[PostImage] [varchar](255) NULL,
	[ImageCaption] [varchar](500) NULL,
	[FeaturedPost] [bit] DEFAULT 0,
	[Content] [text] NOT NULL,
	[PostTags] [varchar](500) NULL,
	[Status] [int] DEFAULT 0,
	[SEOTitle] [varchar](250) NULL,
	[SEODescription] [varchar](250) NULL,
	[SEOKeywords] [varchar](250) NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[Posts] ADD CONSTRAINT unique_post_id UNIQUE (PostID);
GO

INSERT INTO [Posts]
([PostID],[PostType],[Slug],[Author],[Categories],[Title],[ShortDescription],[PostImage],[ImageCaption],[FeaturedPost],[Content] ,[PostTags],[Status],[SEOTitle],[SEODescription],[SEOKeywords],[UpdatedAt],[CreatedAt])
VALUES
(NEWID(), 'Default', 'education-sector', '41924010-149f-4f07-9821-b3998ffaebba', 'Education, Awareness', 'Educating the Future', 'Conventionally, the term ‘education’ is reserved for imparting knowledge to members of the younger generation', 'education-post-image.jpg', NULL, '1', '<p>Conventionally, the term ‘education’ is reserved for imparting knowledge to members of the younger generation. AWARE believes that it is imperative to promote respect for nature, and compassion for all animal life from a very early age. To this end, with every interactive sterilisation campaign, educational materials about animals are currently handed out to disadvantaged children, generally at a local rural school talk.</p>
', 'Education, Awareness, Future', 1, 'Educating the Future',  'Educating the Future','Education,Awareness,Future', '2021-03-30 23:00:00.000', '2021-03-30 23:00:00.000'),

('c1f62a41-2cb6-4d2c-8e98-2190b947c33e', 'Default', 'all-children-deserve-chance', '41924010-149f-4f07-9821-b3998ffaebba', 'OurWork, Education, ChildsRight', 'All children deserve a fair chance in life.', 'Explore how UNICEF works for the fulfilment of every right for every child in The Gambia', 'every-child-post-image.jpg', 'For every child, every right', 1, '<p>In The Gambia, UNICEF works with the Government to deliver services for children across the country. Guided by the principles of the Convention on the Rights of the Child, UNICEF works to ensure every child survives and thrives, and is protected from violence, discrimination and all forms of harmful traditional practices.</p>
<p>Since 1965, we have partnered with the Government of The Gambia to strengthen national laws and policies and empower local communities to deliver better services for women and children. Our interventions are based on our Country Programme,&nbsp;signed with the Government of The Gambia. We implement our programmes in line with the countrys National Development Plan 2017-2021, the United Nations Development Assistance Framework, and the Sustainable Development Goals.&nbsp;</p>
<p>To reach the most vulnerable communities and make the biggest impact, weve deployed our <strong><em>Ñsa Kenno </em></strong>(We can do it) model which is driven by the people we serve, and centres on community engagement as an integral part of a successful programming for change in behaviours and the creation of&nbsp;demand necessary to&nbsp;improve&nbsp;the health, social, and overall well-being of children in The Gambia. The model is based on the human-rights based approach to sustainable development, and is focused on delivering community-level services through the existing local governance structures with the inclusion and participation of children, youth, and other community members.&nbsp;</p>
', 'OurWork, Education, ChildsRight', 1, 'All children deserve a fair chance in life',  'All children deserve a fair chance in life','OurWork,Education,ChildsRight', '2021-03-30 23:00:00.000', '2021-03-30 23:00:00.000'),

(NEWID(), 'Default', 'drinking-water', '41924010-149f-4f07-9821-b3998ffaebba', 'Water, Agriculture', '1 in 3 people globally do not have access to safe drinking water – UNICEF', 'New report on inequalities in access to water, sanitation and hygiene also reveals more than half of the world does not have access to safe sanitation services', 'drinking-water-post-image.jpg', 'In 2017, 71% of the global population (5.3 billion people) used a safely managed drinking-water service', 1, '<h2> Introduction </h2>
<p><span>Safe and readily available water is important for public health, whether it is used for drinking, domestic use, food production or recreational purposes. Improved water supply and sanitation, and better management of water resources, can boost countries’ economic growth and can contribute greatly to poverty reduction.</span></p><p>
<span>In 2010, the UN General Assembly explicitly recognized the human right to water and sanitation. Everyone has the right to sufficient, continuous, safe, acceptable, physically accessible, and affordable water for personal and domestic use.</span></p>
<h2 class="section_head1">Drinking water services</h2>
<p>Sustainable Development Goal target 6.1 calls for universal and equitable access to safe and affordable drinking water. The target is tracked with the indicator of “safely managed drinking water services” – drinking water from an improved water source that is located on premises, available when needed, and free from faecal and priority chemical contamination.</p><p>
', 'Aid, Water, Agriculture', 1, 'Clean, safe drinking water changes everything.',  'Clean, safe drinking water changes everything.','Aid,Water,Agriculture', '2021-03-30 23:00:00.000', '2021-03-30 23:00:00.000');  



DROP TABLE IF EXISTS Category 								 
CREATE TABLE [dbo].[Category](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [varchar](250) NOT NULL,
	[CategoryName] [varchar](250) NULL,
	[ShortCategoryName] [varchar](250) NULL,
	[ShortDescription] [text] NULL,
	[Icon] [varchar](250) NULL,
	[Order] [int] DEFAULT 10,
	[Status] [int] DEFAULT 0,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[Category] ADD CONSTRAINT unique_category_id UNIQUE (CategoryID);
GO

INSERT INTO [Category]
([CategoryID], [CategoryName], [ShortCategoryName], [ShortDescription], [Icon], [Order], [Status], [UpdatedAt])
VALUES
(NEWID(), 'Education', 'Education', 'Education Category', NULL, 1, 1, GETDATE()),
(NEWID(), 'Child Aid', 'ChildAid', 'Child Aid Category', NULL, 2, 1, GETDATE()),
(NEWID(), 'Wild Life', 'WildLife', 'Wild Life Category', NULL, 3, 1, GETDATE()),
(NEWID(), 'Conservation', 'Conservation', 'Conservation Category', NULL, 4, 1, GETDATE()),
(NEWID(), 'Water', 'Water', 'Water Category', NULL, 5, 1, GETDATE()),
(NEWID(), 'Food Aid', 'FoodAid', 'Food Aid Category', NULL, 6, 1, GETDATE()),
(NEWID(), 'Healthcare', 'Healthcare', 'Healthcare Category', NULL, 7, 1, GETDATE()),
(NEWID(), 'Women Empowerment', 'WomenEmpowerment', 'Women Empowerment Category', NULL, 8, 1, GETDATE());



/* Navigation */
DROP TABLE IF EXISTS Navigation 
CREATE TABLE [dbo].[Navigation](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NavigationID] [varchar](255) NOT NULL,
	[NavigationName] [varchar](255) NOT NULL,
	[NavigationLink] [varchar](255) NULL,
	[Order] [int] DEFAULT 10,
	[Parent] [varchar](255) NULL,
	[Status] [int] DEFAULT 0,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[Navigation] ADD CONSTRAINT unique_navigation_id UNIQUE (NavigationID);
GO
ALTER TABLE [dbo].[Navigation] ADD CONSTRAINT unique_navigation_name UNIQUE (NavigationName);
GO

INSERT INTO [Navigation]
([NavigationID], [NavigationName], [NavigationLink], [Order], [Parent], [Status], [UpdatedAt])
VALUES
(NEWID(), 'Home', '#hero', '1', NULL, '1', GETDATE()),
(NEWID(), 'About', '#about', '2', NULL, '1', GETDATE()),
(NEWID(), 'Services', '#services', '3', NULL, '1', GETDATE()),
(NEWID(), 'Portfolio', '#portfolio', '4', NULL, '1', GETDATE()),
(NEWID(), 'Team', '#team', '5', NULL, '1', GETDATE()),
(NEWID(), 'Posts', '/Posts', '6', NULL, '1', GETDATE()),
(NEWID(), 'Contact', '#contact', '7', NULL, '1', GETDATE());


/* FooterNavigation */
DROP TABLE IF EXISTS FooterNavigation 
CREATE TABLE [dbo].[FooterNavigation](
    [ID] [int] IDENTITY(1,1) NOT NULL,
    [NavigationID] [varchar](255) NOT NULL,
    [NavigationName] [varchar](255) NOT NULL,
    [NavigationLink] [varchar](255) NULL,
    [Order] [int] DEFAULT 10,
    [Status] [int] DEFAULT 0,
    [UpdatedAt] [datetime] NULL,
    [CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[FooterNavigation] ADD CONSTRAINT unique_footer_navigation_id UNIQUE (NavigationID);
GO
ALTER TABLE [dbo].[FooterNavigation] ADD CONSTRAINT unique_footer_navigation_name UNIQUE (NavigationName);
GO                                                                                                                                                                                        


INSERT INTO [FooterNavigation]
([NavigationID], [NavigationName], [NavigationLink], [Order], [Status], [UpdatedAt])
VALUES
(NEWID(), 'Home', '#hero', '1', '1', GETDATE()),
(NEWID(), 'About', '#about', '2', '1', GETDATE()),
(NEWID(), 'Services', '#services', '3', '1', GETDATE()),
(NEWID(), 'Portfolio', '#portfolio', '4', '1', GETDATE()),
(NEWID(), 'Team', '#team', '5', '1', GETDATE()),
(NEWID(), 'Posts', '/Posts', '6', '1', GETDATE()),
(NEWID(), 'Contact', '#contact', '7', '1', GETDATE());



/* Pages */
DROP TABLE IF EXISTS Pages 
CREATE TABLE [dbo].[Pages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PageID] [varchar](255) NOT NULL,
	[Slug] [varchar](255) NOT NULL,
	[Author] [varchar](255) NOT NULL,
	[Title] [varchar](255) NOT NULL,
	[ShortDescription] [varchar](500) NULL,
	[PageImage] [varchar](255) NULL,
	[ImageCaption] [varchar](500) NULL,
	[Content] [text] NOT NULL,
	[Status] [int] DEFAULT 0,
	[SEOTitle] [varchar](250) NULL,
	[SEODescription] [varchar](250) NULL,
	[SEOKeywords] [varchar](250) NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[Pages] ADD CONSTRAINT unique_page_id UNIQUE (PageID);
GO

INSERT INTO [Pages]
([PageID], [Slug], [Author], [Title], [ShortDescription], [PageImage], [ImageCaption], [Content], [Status], [SEOTitle], [SEODescription], [SEOKeywords], [UpdatedAt], [CreatedAt])
VALUES
(NEWID(), 'take-action', '41924010-149f-4f07-9821-b3998ffaebba', 'Deliver Health & Hope to the Neediest People in the World', 'Deliver Health & Hope to the Neediest People in the World', 'health-page-image.jpg', 'Midwife checks the blood pressure of a patient, who delivered her baby. During the COVID-19 pandemic', '<p class="text-dark font-weight-bold">There are so many ways to get involved with Project C.U.R.E. You can donate medical supplies and equipment, volunteer in one of our six large warehouses, deliver C.U.R.E. Kits when traveling abroad, pack Kits for Kids with your Rotary Club, school or corporate group, attend one of our amazing events, or travel with us on a Clinics trip. Your skills and talents will help us deliver health &amp; hope to the neediest people in the world.</p>', 1, 'Deliver Health & Hope to the Neediest People in the World', 'Deliver Health & Hope to the Neediest People in the World', 'Health, Hope, People, World', '2021-03-30 23:00:00.000', '2021-03-30 23:00:00.000'),

('57928261-c3f6-4cb4-baf6-194c067a8a93', 'healthy-people-better-world', '41924010-149f-4f07-9821-b3998ffaebba', 'Healthy People. Better World', 'That means all people - regardless of politics, religion, or ability to pay. Improving their lives is Direct Reliefs mission', 'healthy-people-page-image.jpg', NULL, '<span>Direct Relief is a humanitarian aid organization, active in all 50 states and more than 80 countries, with a mission to improve the health and lives of people affected by poverty or emergencies – without regard to politics, religion, or ability to pay.</span><h2 class="module__title">Humanitarian Medical Aid</h2> <div class="module__description"><span>Direct Reliefs assistance programs are tailored to&nbsp;the particular circumstances and needs of the worlds most vulnerable and at-risk populations. This tradition of direct and targeted assistance, provided in a manner that respects and involves the people served, has been a hallmark of the organization since its founding in 1948 by refugee war immigrants to the U.S.</span></div>', 1, 'Healthy People. Better World', 'Healthy People. Better World', 'Health, Better, People, World', '2021-03-30 23:00:00.000', '2021-03-30 23:00:00.000'),

(NEWID(), 'clean-water', '41924010-149f-4f07-9821-b3998ffaebba', 'Water Crisis - Learn About The Global Water Crisis', 'Water connects every aspect of life. Access to safe water and sanitation can quickly turn problems into potential – empowering people with time for school and work, and contributing to improved health for women, children, and families around the world.', 'clean-water-page-image.jpg', NULL, '<p class="font-bold mission-text">There are <span class="striked-out" data-sentence="1" data-split-animation="true" style="background-size: 0% 67%;"><span>millions of</span></span> people who <span class="striked-out" data-sentence="1" data-split-animation="true" style="background-size: 0% 67%;"><span>don’t</span></span> have access to clean water. They <span class="striked-out" data-sentence="2" data-split-animation="true" style="background-size: 0% 67%;"><span>get diseases</span></span> like <span class="striked-out" data-sentence="2" data-split-animation="true" style="background-size: 0% 67%;"><span>dysentery, which prevent them from</span></span> staying healthy and going to school. They have <span class="striked-out" data-sentence="3" data-split-animation="true" style="background-size: 0% 67%;"><span>no</span></span> time for a career and <span class="striked-out" data-sentence="3" data-split-animation="true" style="background-size: 0% 67%;"><span>zero chance to</span></span> live in prosperity. Their kids are born into the same <span class="striked-out" data-sentence="4" style="background-size: 0% 67%;"><span>cycle of poverty and dis</span></span>advantage.</p>', 1, 'Cross out the water crisis and build a better world', 'Cross out the water crisis and build a better world', 'Water, World, Health, Future', '2021-03-30 23:00:00.000', '2021-03-30 23:00:00.000'),

(NEWID(), 'about-us', '41924010-149f-4f07-9821-b3998ffaebba', 'About Us', NULL, NULL, NULL, '<p class="text-dark font-weight-bold">There are so many ways to get involved with Project C.U.R.E. You can donate medical supplies and equipment, volunteer in one of our six large warehouses, deliver C.U.R.E. Kits when traveling abroad, pack Kits for Kids with your Rotary Club, school or corporate group, attend one of our amazing events, or travel with us on a Clinics trip. Your skills and talents will help us deliver health &amp; hope to the neediest people in the world.</p>', 1, 'About Us', 'About Us', 'About Us, Who are we?, What we do?', '2021-03-30 23:00:00.000', '2021-03-30 23:00:00.000');




/* GalleryImages */
DROP TABLE IF EXISTS GalleryImages 
CREATE TABLE [dbo].[GalleryImages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [varchar](255) NOT NULL,
	[Image] [varchar](255) NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)

INSERT INTO [GalleryImages]
([ParentID], [Image], [UpdatedAt])
VALUES
('c1f62a41-2cb6-4d2c-8e98-2190b947c33e', 'children-deserve-chance-01.jpg', GETDATE()),
('c1f62a41-2cb6-4d2c-8e98-2190b947c33e', 'children-deserve-chance-02.jpg', GETDATE()),
('c1f62a41-2cb6-4d2c-8e98-2190b947c33e', 'children-deserve-chance-03.jpg', GETDATE()),
('57928261-c3f6-4cb4-baf6-194c067a8a93', 'healthy-people-gallery-01.jpg', GETDATE()),
('57928261-c3f6-4cb4-baf6-194c067a8a93', 'healthy-people-gallery-02.jpg', GETDATE()),
('57928261-c3f6-4cb4-baf6-194c067a8a93', 'healthy-people-gallery-03.jpg', GETDATE()),

('c1051a1d-50db-4023-8dc6-ddf09876f17d', 'project-cure-image-01.jpg', GETDATE()),
('c1051a1d-50db-4023-8dc6-ddf09876f17d', 'project-cure-image-02.jpg', GETDATE()),
('688518ab-2f4f-4843-b5f7-0ee9d401ee66', 'project-child-aid-image-01.jpg', GETDATE()),
('688518ab-2f4f-4843-b5f7-0ee9d401ee66', 'project-child-aid-image-02.jpg', GETDATE()),
('746f013b-99eb-4752-876b-b32917fb86de', 'social-impact-image-01.jpg', GETDATE()),
('746f013b-99eb-4752-876b-b32917fb86de', 'social-impact-image-02.jpg', GETDATE()),
('ca98255f-8229-4cba-940e-ab3cc7fa5f08', 'wildlife-foundation-image-01.jpg', GETDATE()),
('ca98255f-8229-4cba-940e-ab3cc7fa5f08', 'wildlife-foundation-image-02.jpg', GETDATE()),
('15bf2f96-1652-4879-8c23-b8d99eeb7993', 'gates-foundation-image-01.jpg', GETDATE()),
('15bf2f96-1652-4879-8c23-b8d99eeb7993', 'gates-foundation-image-02.jpg', GETDATE()),
('ce879eae-1e89-4a16-9317-d5091044e7e6', 'invisible-children-image-01.jpg', GETDATE()),
('ce879eae-1e89-4a16-9317-d5091044e7e6', 'invisible-children-image-02.jpg', GETDATE());


/* Videos */
DROP TABLE IF EXISTS Videos
CREATE TABLE [dbo].[Videos](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[VideoID] [varchar](255) NOT NULL,
	[Title] [varchar](255) NOT NULL,
	[Link] [varchar](255) NULL,
	[Status] [int] DEFAULT 0,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[Videos] ADD CONSTRAINT unique_video_id UNIQUE (VideoID);
GO


/* Services */
DROP TABLE IF EXISTS Services 
CREATE TABLE [dbo].[Services](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceID] [varchar](255) NOT NULL,
	[ServiceTitle] [varchar](255) NOT NULL,
	[ShortDescription] [varchar](500) NULL,
	[ServiceLink] [varchar](255) NULL,
	[ServiceIcon] [varchar](255) NULL,
	[Status] [int] DEFAULT 0,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[Services] ADD CONSTRAINT unique_service_id UNIQUE (ServiceID);
GO
ALTER TABLE [dbo].[Services] ADD CONSTRAINT unique_service_title UNIQUE (ServiceTitle);
GO
INSERT INTO [Services]
([ServiceID], [ServiceTitle], [ShortDescription], [ServiceLink], [ServiceIcon], [Status], [UpdatedAt])
VALUES
(NEWID(), 'Humanitarian Medical Aid', 'Direct Reliefs assistance programs are tailored to the particular circumstances and needs of the worlds most vulnerable and at-risk populations.', NULL, 'bi bi-shield-fill-plus', 1, GETDATE()),

(NEWID(), 'Covid-19 Relief', 'Working with experts to promote facts over fear, bringing reliable guidance to parents, caregivers and educators, and partnering with front-line responders to ensure they have the information and resources they need to keep children healthy and learning.', NULL, 'bi bi-flower2', 1, GETDATE()),

(NEWID(), 'Disaster Relief', 'Preparing the most vulnerable communities worldwide for more frequent, more destructive emergencies. ', NULL, 'bi bi-truck', 1, GETDATE());



                                                                                                                     

/* Portfolio */
DROP TABLE IF EXISTS Portfolio 
CREATE TABLE [dbo].[Portfolio](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PortfolioID] [varchar](255) NOT NULL,
	[PortfolioTitle] [varchar](255) NOT NULL,
	[Slug] [varchar](255) NOT NULL,
	[Image] [varchar](255) NOT NULL,
	[Category] [varchar](255) NULL,
	[PortfolioContent] [text] NULL,
	[Status] [int] DEFAULT 0,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[Portfolio] ADD CONSTRAINT unique_portfolio_id UNIQUE (PortfolioID);
GO
ALTER TABLE [dbo].[Portfolio] ADD CONSTRAINT unique_portfolio_title UNIQUE (PortfolioTitle);
GO

INSERT INTO [Portfolio]
([PortfolioID], [PortfolioTitle], [Slug], [Image], [PortfolioContent], [Status], [UpdatedAt], [CreatedAt])
VALUES
('c1051a1d-50db-4023-8dc6-ddf09876f17d', 'Project C.U.R.E.', 'project-cure', 'project-cure-image.jpg', '<h1>Delivering Health &amp; Hope</h1><div class="description">Project C.U.R.E. is the world’s largest distributor of donated medical equipment and supplies to resource-limited communities across the globe, touching the lives of patients, families, and children in more than 135 countries.</div><div class="buttons-container"><a href="https://projectcure.org/take-action/" target="" class="button default">Take Action</a><a href="https://projectcure.org/our-work/medical-cargo-containers/cargo-request-form/" target="" class="button light">Request Medical Supplies</a><a href="https://projectcure.org/take-action/events/pinot-with-the-president/" target="" class="button default">Pinot with the President</a></div>', 1, '2021-03-30 23:00:00.000', '2021-03-30 23:00:00.000'),

('688518ab-2f4f-4843-b5f7-0ee9d401ee66', 'Child Aid', 'child-aid-work', 'project-child-aid-image.jpg', '<h2 class="has-text-align-center">Going to School is Not the Same as Learning</h2><p class="has-text-align-center padding-btm">An education crisis grips the&nbsp;globe. Children go to school and don’t learn. Roughly 262 million primary school children worldwide show up for school most days but fail to learn basic foundational literacy skills, <a href="https://sdg.uis.unesco.org/2017/09/21/new-data-reveal-a-learning-crisis-that-threatens-development-around-the-world/">according to UNESCO</a>. Without the ability to read, write and comprehend basic texts, at-risk children fall deeper into cyclical poverty with no obvious way out. </p>', 1, '2021-03-30 23:00:00.000', '2021-03-30 23:00:00.000'),

('746f013b-99eb-4752-876b-b32917fb86de', 'Delivering Bold and Thoughtful Solutions', 'social-impact', 'social-impact-image.jpg', '<p>A social impact agency and strategic partner, specializing in branding, digital and marketing solutions for <a href="https://briteweb.com/clients/">nonprofits, foundations and purpose-driven companies</a>.</p> <p>We’re experts at designing and guiding tailored processes that deliver tangible results and powerful insights.</p> <p>With offices in Vancouver and New York and a carefully curated <a href="https://briteweb.com/network/">network</a> of global social impact professionals, we offer the familiarity and attentiveness of a local partner with the breadth of a global agency.</p>', 1, '2021-03-30 23:00:00.000', '2021-03-30 23:00:00.000'),

('ca98255f-8229-4cba-940e-ab3cc7fa5f08', 'Wildlife Foundation', 'wildlife-foundation', 'wildlife-foundation-image.jpg', '<p style="text-align: center; font-size: 25px;"><strong>The Art of Survival:</strong> FIGHT • PROTECT • ENGAGE</p> <p>As we witness staggering rates of global biodiversity loss, environmental and wildlife protection has never been more important in safeguarding the future health of our planet.</p> <p>Our mission is to&nbsp;<strong>Fight</strong>,&nbsp;<strong>Protect,</strong>&nbsp;and&nbsp;<strong>Engage</strong>&nbsp;on behalf of endangered wildlife around the world.</p> <p>Our ground-based conservation partners, operating on the front line in the fight against wildlife crime, are protecting some of the world’s last wildlife strongholds and pristine wilderness landscapes. As the climate catastrophe escalates, pressure on these dwindling wildlife populations and locations grows. DSWF is at the heart of enabling and ensuring wildlife is given the fighting chance it needs to stabilise and recover.</p>', 1, '2021-03-30 23:00:00.000', '2021-03-30 23:00:00.000'),

('15bf2f96-1652-4879-8c23-b8d99eeb7993', 'Gates Foundation', 'gates-foundation', 'gates-foundation-image.jpg', '<p>We share two values that are a big part of why we started our foundation. Both of us were taught to give back and to be optimistic about the future.</p><p>From early childhood, we each saw how our parents helped out in our local communities, and we were taught that anything is possible. Unfortunately, factors outside of anyone’s control make it hard for some people to reach their potential: things like when they were born, who their parents are, where they grew up, whether they are a boy or a girl.</p> <p>We wake up every day determined to use our resources to create a world where everyone has the opportunity to lead a healthy and productive life. Most importantly, we believe this: All lives have equal value. That’s why we made the decision to donate our wealth from Microsoft to help others.</p><p>The challenge when we started out was how to do that in a meaningful and high-impact way. We were drawn to things that sprang from our experience, so we began donating PCs to public libraries across the United States to give everyone a chance to use one. As we read and traveled more, we also became curious about inequalities further from home.</p>', 1, '2021-03-30 23:00:00.000', '2021-03-30 23:00:00.000'),

('ce879eae-1e89-4a16-9317-d5091044e7e6', 'Invisible Children', 'invisible-children', 'invisible-children-image.jpg', '<h5 class="text-lead text-info"><span class="text-dark">THE INVISIBLE CHILDREN OF TODAY </span>AN UNEXPECTED JOURNEY</h5><p>In 2004, the United Nations called the LRA crisis in northern Uganda the “most forgotten, neglected humanitarian emergency in the world.” Invisible Children was founded to change that and to fight against the false notion that our responsibility to each other stops at our own nations’&nbsp;borders.</p><p>Our founders believed that if people around the world knew the reality of LRA violence -- more than 60,000 children abducted, tens of thousands killed, and millions displaced -- and if they could see the names, faces, and stories behind the statistics, they would be moved to take action and demand justice. They were right. By harnessing the power of storytelling, youth idealism, and human empathy, they mobilized millions around the world to raise the banner for LRA-affected communities and move world leaders to take historic action to end the LRA&nbsp;crisis.</p>', 1, '2021-03-30 23:00:00.000', '2021-03-30 23:00:00.000');



                                                                                                                                                                                                               

/* Team */
DROP TABLE IF EXISTS Team 
CREATE TABLE [dbo].[Team](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TeamID] [varchar](255) NOT NULL,
	[FirstName] [varchar](255)  NOT NULL,
	[LastName] [varchar](255) NOT NULL,
	[Title] [varchar](255) NOT NULL,
	[ProfileImage] [varchar](250) NULL,
	[Link] [varchar](255) NULL,
	[Facebook] [varchar](250) NULL,
	[Twitter] [varchar](250) NULL,
	[Instagram] [varchar](250) NULL,
	[LinkedIn] [varchar](250) NULL,
	[Status] [int] DEFAULT 0,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[Team] ADD CONSTRAINT unique_team_id UNIQUE (TeamID);
GO

INSERT INTO [Team]
([TeamID], [FirstName], [LastName], [Title], [ProfileImage], [Facebook], [Twitter], [Instagram], [LinkedIn], [Status], [UpdatedAt])
VALUES
(NEWID(), 'Aisha', 'Ceesay', 'President and CEO', 'aisha-ceesay.jpg', 'https://www.facebook.com/', 'https://www.twitter.com/', 'https://www.instagram.com/', 'https://www.linkedin.com/', 1, GETDATE()),
(NEWID(), 'John', 'Mendy', 'Administrative Director', 'john-mendy.jpg', 'https://www.facebook.com/', 'https://www.twitter.com/', 'https://www.instagram.com/', 'https://www.linkedin.com/', 1, GETDATE()),
(NEWID(), 'Binta', 'Kebbeh', 'CPA', 'binta-kebbeh.jpg', 'https://www.facebook.com/', 'https://www.twitter.com/', 'https://www.instagram.com/', 'https://www.linkedin.com/', 1, GETDATE()),
(NEWID(), 'Omar', 'Camara', 'Finance', 'omar-camara.jpg', 'https://www.facebook.com/', 'https://www.twitter.com/', 'https://www.instagram.com/', 'https://www.linkedin.com/', 1, GETDATE());



                                                                                                             

/* Partner */
DROP TABLE IF EXISTS Partner 
CREATE TABLE [dbo].[Partner](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PartnerID] [varchar](255) NOT NULL,
	[PartnerName] [varchar](255) NOT NULL,
	[Logo] [varchar](255) NOT NULL,
	[Link] [varchar](255) NULL,
	[Status] [int] DEFAULT 0,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[Partner] ADD CONSTRAINT unique_partner_id UNIQUE (PartnerID);
GO

INSERT INTO [Partner]
([PartnerID], [PartnerName], [Logo], [Link], [Status], [UpdatedAt])
VALUES
(NEWID(), 'World Health Organization', 'who.jpg', 'https://www.who.int/', 1, GETDATE()),
(NEWID(), 'International Federation of Red Cross and Red Crescent', 'ifrc.jpg', 'https://media.ifrc.org/ifrc/', 1, GETDATE()),
(NEWID(), 'Save The Children', 'save-the-children.jpg', 'https://www.savethechildren.net/', 1, GETDATE()),
(NEWID(), 'World Food Program', 'wfp.jpg', 'https://www.wfp.org/', 1, GETDATE()),
(NEWID(), 'United Nations Children Fund', 'unicef.jpg', 'https://www.unicef.org/', 1, GETDATE()),
(NEWID(), 'Covid Relief Fund', 'crf.jpg', 'https://www.directrelief.org/emergency/coronavirus-outbreak/', 1, GETDATE()),
(NEWID(), 'Islamic Relief Fund', 'irf.jpg', 'https://www.islamic-relief.org/', 1, GETDATE());


                                                                                                                                                                                                                                           

/* Contact */
DROP TABLE IF EXISTS Contact 
CREATE TABLE [dbo].[Contact](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ContactID] [varchar](255) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Email] [varchar](255) NULL,
	[Phone] [varchar](255) NULL,
	[Subject] [varchar](255) NULL,
	[Message] [text] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[Contact] ADD CONSTRAINT unique_contact_id UNIQUE (ContactID);
GO

INSERT INTO [Contact]
([ContactID], [Name], [Email], [Phone], [Subject], [Message], [UpdatedAt])
VALUES
(NEWID(), 'John Doe', 'johndoe@email.com', '+1 200 827 36482', 'Enquiry','I have read about your company on website and would like to do research on your company. I want to know about the marketing process. Therefore I request you to give me one day permission of visiting your company and allow me to inquire about the marketing process.', GETDATE());


                                                                                                                                                                                                                                                                                                                      

/* MessageViews */
DROP TABLE IF EXISTS MessageViews 
CREATE TABLE [dbo].[MessageViews](
	[ViewID] [varchar](255) PRIMARY KEY NOT NULL,
	[ContactID] [varchar](255) NOT NULL,
	[AccountID] [varchar](255) NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[MessageViews] ADD CONSTRAINT unique_view_id UNIQUE (ViewID);
GO



                                                                         
/* FAQ */
DROP TABLE IF EXISTS FAQ 
CREATE TABLE [dbo].[FAQ](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FaqID] [varchar](255) NOT NULL,
	[Question] [varchar](255) NOT NULL,
	[Answer] [text] NOT NULL,
	[Status] [int] DEFAULT 0,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[FAQ] ADD CONSTRAINT unique_question UNIQUE (Question);
GO

INSERT INTO [FAQ]
([FaqID], [Question], [Answer], [Status], [UpdatedAt])
VALUES
(NEWID(), 'How can I learn more about my donation?', 'Once you’ve donated on our website, you will receive an email from us within 8-12 weeks of your gift date. You’ll hear from girls’ voices directly on the issues and topics that matter most to them, as a representation of your investment and the larger community of girls we serve. Additional information, such as frequency of communication and what to expect moving forward, will be included in this initial correspondence.', 1, GETDATE()),
(NEWID(), 'Who are we?', 'We are a non-profit organization bringing clean and safe drinking water to people in developing countries.', 1, GETDATE()),
(NEWID(), 'Our Work?', '785 million people lack basic access to clean and safe drinking water. We’ve been on a mission to end the water crisis since 2006, and with the help of generous supporters like you, we’re getting closer every day.', 1, GETDATE());





/* Testimonial */
DROP TABLE IF EXISTS Testimonials 
CREATE TABLE [dbo].[Testimonials](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TestimonialID] [varchar](255) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Title] [varchar](255) NOT NULL,
	[Organization] [varchar](255) NULL,
	[ProfileImage] [varchar](255) NULL,
	[Testimonial] [varchar](1000) NOT NULL,
	[Status] [int] DEFAULT 0,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[Testimonials] ADD CONSTRAINT unique_testimonial_id UNIQUE (TestimonialID);
GO


INSERT INTO [Testimonials]
([TestimonialID], [Name], [Title], [Organization], [ProfileImage], [Testimonial], [Status], [UpdatedAt])
VALUES
(NEWID(), 'Lamin Sarr', 'CEO ', 'Charity Organization', 'lamin-sarr.jpg', 'Sometimes, when you come off the phone, you feel like you have made someone’s day, just by calling them. Some people feel that their problem isn’t big enough to bother people with, but when asked how they are during this regular contact, they open up. Also, this call makes people aware of products and services they may not know about', 1, GETDATE()),
(NEWID(), 'Naha Paraha', 'Lead Advisor ', 'Team Charity', 'naha-paraha.jpg', 'Charity Challenge offers a unique opportunity for people to challenge themselves and get fit, while raising money for a great cause. Since AC-AF works primarily in East Africa, we are particularly excited by the Kilimanjaro Summit Climb, although all of the challenges offer a one-of- a- kind experience.', 1, GETDATE()),
(NEWID(), 'Charlene Soll', 'Director ', 'Help Charity', 'charlene-soll.jpg', 'We have worked closely with Charity Escapes for over two years now and it works fantastically well for us. We have used several of their prizes for our auctions at various events and it really helps with the process of sourcing items which can be very difficult and time consuming.', 1, GETDATE());



                                                                                                                                                                                                                                                                                                                                                                  
/* HomeSliders */
DROP TABLE IF EXISTS HomeSliders 
CREATE TABLE [dbo].[HomeSliders](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SliderID] [varchar](255) NOT NULL,
	[SliderTitle] [varchar](255) NOT NULL,
	[SubText] [text] NULL,
	[SliderLink] [varchar](255) NULL,
	[SliderImage] [varchar](255) NULL,
	[Status] [int] DEFAULT 0,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[HomeSliders] ADD CONSTRAINT unique_slider_id UNIQUE (SliderID);
GO

ALTER TABLE [dbo].[HomeSliders] ADD CONSTRAINT unique_slider_title UNIQUE (SliderTitle);
GO

INSERT INTO [HomeSliders]
([SliderID], [SliderTitle], [SubText], [SliderLink], [SliderImage], [Status], [UpdatedAt])
VALUES
(NEWID(), 'Deliver Health & Hope to the Neediest People in the World', 'We are an international charity that changes lives through the compassion and commitment of dedicated staff and volunteers', '/Page/take-action', 'health-slider-image.jpg', 1, GETDATE()),
(NEWID(), 'Healthy People. Better World', 'That means all people - regardless of politics, religion, or ability to pay. Improving their lives is Direct Reliefs mission', '/Page/healthy-people-better-world', 'healthy-people-slider-image.jpg', 1, GETDATE()),
(NEWID(), 'Learn About The Global Water Crisis', 'Water connects every aspect of life. Access to safe water and sanitation can quickly turn problems into potential', '/Page/clean-water', 'clean-water-slider-image.jpg', 1, GETDATE());



                                                                                                                                                                                                                                                                                                                          

/* ThemeSettings */
DROP TABLE IF EXISTS ThemeSettings 
CREATE TABLE [dbo].[ThemeSettings](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ThemeSettingID] [varchar](255) NOT NULL,
	[SettingName] [varchar](255) NOT NULL,
	[SettingType] [varchar](255) NULL,
	[SettingValue] [varchar](500) NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[ThemeSettings] ADD CONSTRAINT unique_theme_setting_id UNIQUE (ThemeSettingID);
GO

ALTER TABLE [dbo].[ThemeSettings] ADD CONSTRAINT unique_theme_setting_name UNIQUE (SettingName);
GO

INSERT INTO [ThemeSettings]
([ThemeSettingID], [SettingName], [SettingType], [SettingValue], [UpdatedAt])
VALUES
(NEWID(), 'ThemeColor', 'ColorSettings', '#ED502F', GETDATE()),
(NEWID(), 'SelectedTheme', 'ThemeTemplate', 'Default', GETDATE());

                                                                                                                                     

/* ContentManagement */
DROP TABLE IF EXISTS ContentManagement 
CREATE TABLE [dbo].[ContentManagement](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ContentID] [varchar](255) NOT NULL,
	[ContentName] [varchar](255) NOT NULL,
	[ContentGroup] [varchar](255) NOT NULL,
	[ContentValue] [text] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[ContentManagement] ADD CONSTRAINT unique_content_id UNIQUE (ContentID);
GO

ALTER TABLE [dbo].[ContentManagement] ADD CONSTRAINT unique_content_name UNIQUE (ContentName);
GO

INSERT INTO [ContentManagement]
([ContentID], [ContentName], [ContentGroup], [ContentValue], [UpdatedAt])
VALUES
(NEWID(), 'OrganizationName', 'Organization', 'Help Charity Intl.', GETDATE()),
(NEWID(), 'OrganizationAddress', 'Organization', 'Fajara, M Section, The Gambia', GETDATE()),
(NEWID(), 'PhoneNumber', 'Organization', '220 7654321', GETDATE()),
(NEWID(), 'OptionalPhoneNumber', 'Organization', '', GETDATE()),
(NEWID(), 'Email', 'Organization', 'info@helpcharity.com', GETDATE()),
(NEWID(), 'Logo', 'Organization', 'default-logo.jpg', GETDATE()),
(NEWID(), 'LogoFormat', 'Organization', '1', GETDATE()),
(NEWID(), 'LogoWidth', 'Organization', '100', GETDATE()),
(NEWID(), 'Favicon', 'Organization', 'default-favicon.jpg', GETDATE()),
(NEWID(), 'FacebookLink', 'SocialInfo', 'https://www.facebook.com/', GETDATE()),
(NEWID(), 'TwitterLink', 'SocialInfo', 'https://www.twitter.com/', GETDATE()),
(NEWID(), 'InstagramLink', 'SocialInfo', 'https://www.instagram.com/', GETDATE()),
(NEWID(), 'YouTubeLink', 'SocialInfo', 'https://www.youtube.com/', GETDATE()),
(NEWID(), 'LinkedInLink', 'SocialInfo', 'https://www.linkedin.com/', GETDATE()),
(NEWID(), 'AboutSummary', 'AboutInfo', 'OUR VISION Neither the markets nor aid alone can solve the problems of poverty. More than two billion people around the world lack access to basic goods and services—from clean water and electricity to an education and the freedom to participate in the economy. We’re here to change that. Our vision is a world based on dignity, where every human being has the same opportunity. Rather than giving philanthropy away, we invest it in companies and change makers.', GETDATE()),
(NEWID(), 'AboutSummaryLink', 'AboutInfo', '/Page/about-us', GETDATE()),
(NEWID(), 'HomeVideo', 'AboutInfo', 'https://www.youtube.com/watch?v=U1z-uhNZeAA', GETDATE()),
(NEWID(), 'HomeVideoTitle', 'AboutInfo', 'What’s New In .NET Core 3 0', GETDATE()),
(NEWID(), 'HomeVideoDescription', 'AboutInfo', 'See what makes .NET Core 3.0 such a big deal for .NET developers no matter what kind of apps you’re building. See the productivity and performance improvements and innovative new ways to build and enhance your .NET applications.', GETDATE()),
(NEWID(), 'AboutUs', 'HeaderTexts', 'ABOUT US', GETDATE()),
(NEWID(), 'Services', 'HeaderTexts', 'OUR SERVICES', GETDATE()),
(NEWID(), 'Testimonials', 'HeaderTexts', 'TESTIMONIALS', GETDATE()),
(NEWID(), 'Posts', 'HeaderTexts', 'OUR RECENT POSTS', GETDATE()),
(NEWID(), 'Donate', 'HeaderTexts', 'Your Support is Powerful', GETDATE()),
(NEWID(), 'Portfolio', 'HeaderTexts', 'CHECK OUR PORTFOLIO', GETDATE()),
(NEWID(), 'Team', 'HeaderTexts', 'OUR TEAM', GETDATE()),
(NEWID(), 'FAQ', 'HeaderTexts', 'FREQUENTLY ASKED QUESTIONS', GETDATE()),
(NEWID(), 'Partners', 'HeaderTexts', 'OUR PARTNERS', GETDATE()),
(NEWID(), 'ContactUs', 'HeaderTexts', 'CONTACT US', GETDATE()),
(NEWID(), 'SEOAuthor', 'SeoSettings', 'GexpoTech', GETDATE()),
(NEWID(), 'SEODescription', 'SeoSettings', 'GexpoTech Company', GETDATE()),
(NEWID(), 'SEOKeywords', 'SeoSettings', 'NGO, Charity, The Gambia, GexpoTech', GETDATE());

                                                                                                                                           

/* Service Stat */
DROP TABLE IF EXISTS ServiceStats 
CREATE TABLE [dbo].[ServiceStats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceStatID] [varchar](255) NOT NULL,
	[Title] [varchar](255) NOT NULL,
	[StatValue] [varchar](255) NULL,
	[Icon] [varchar](255) NULL,
	[Link] [varchar](255) NULL,
	[Order] [int] DEFAULT 10,
	[Status] [int] DEFAULT 0,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[ServiceStats] ADD CONSTRAINT unique_service_stat_id UNIQUE (ServiceStatID);
GO
ALTER TABLE [dbo].[ServiceStats] ADD CONSTRAINT unique_service_stat_title UNIQUE (Title);
GO

INSERT INTO [ServiceStats]
([ServiceStatID], [Title], [StatValue], [Icon], [Link], [Order], [Status], [UpdatedAt])
VALUES
(NEWID(), 'Projects Funded', '232', 'bi bi-info-square', '', '1', '1', GETDATE()),
(NEWID(), 'People Served', '9821', 'bi bi-people-fill', '', '2', '1', GETDATE()),
(NEWID(), 'Locations', '8', 'bi bi-geo-fill', '', '3', '1', GETDATE()),
(NEWID(), 'Volunteers & Hard Workers', '21', 'bi bi-people', '', '4', '1', GETDATE());





                                                                                                                                                                                                                                             
/* Content Display */
DROP TABLE IF EXISTS ContentDisplay 
CREATE TABLE [dbo].[ContentDisplay](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ContentDisplayID] [varchar](255) NOT NULL,
	[ContentName] [varchar](255) NOT NULL,
	[ContentDisplayName] [varchar](255) NULL,
	[DisplayStatus] [bit] DEFAULT 0,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[ContentDisplay] ADD CONSTRAINT unique_content_display_ID UNIQUE (ContentDisplayID);
GO

ALTER TABLE [dbo].[ContentDisplay] ADD CONSTRAINT unique_content_display_name UNIQUE (ContentName);
GO


INSERT INTO [ContentDisplay]
([ContentDisplayID], [ContentName], [ContentDisplayName], [DisplayStatus],[UpdatedAt])
VALUES
(NEWID(), 'HomeSliders', 'Home Sliders', '1', GETDATE()),
(NEWID(), 'SocialMedia', 'Social Media', '1', GETDATE()),
(NEWID(), 'ServiceStatistics', 'Service Statistics', '1', GETDATE()),
(NEWID(), 'AboutUs', 'About Us', '1', GETDATE()),
(NEWID(), 'HomeVideo', 'Home Video', '1', GETDATE()),
(NEWID(), 'Services', 'Services', '1', GETDATE()),
(NEWID(), 'Testimonials', 'Testimonials', '1', GETDATE()),
(NEWID(), 'Posts', 'Posts', '1', GETDATE()),
(NEWID(), 'TwitterFeed', 'TwitterFeed', '1', GETDATE()),
(NEWID(), 'Donate', 'Donate', '1', GETDATE()),
(NEWID(), 'Portfolio', 'Portfolio', '1', GETDATE()),
(NEWID(), 'Team', 'Team', '1', GETDATE()),
(NEWID(), 'FAQ', 'FAQ', '1', GETDATE()),
(NEWID(), 'Partners', 'Partners', '1', GETDATE()),
(NEWID(), 'ContactUs', 'Contact Us', '1', GETDATE()),
(NEWID(), 'FacebookComments', 'Facebook Comments', '1', GETDATE()),
(NEWID(), 'TwitterFeeds', 'Twitter Feeds', '1', GETDATE());




/* SITE VISITS */
DROP TABLE IF EXISTS SiteVisits 
CREATE TABLE [dbo].[SiteVisits](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[VisitID] [varchar](250) NOT NULL,
	[DocumentID] [varchar](250) NULL,
	[DocumentType] [varchar](250) NULL,
	[IpAddress] [varchar](250) NULL,
	[Country] [varchar](250) NULL,
	[Browser] [varchar](250) NULL,
	[Device] [varchar](250) NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
) 
ALTER TABLE [dbo].[SiteVisits] ADD CONSTRAINT unique_site_visit_id UNIQUE (VisitID);
GO

INSERT INTO [SiteVisits]
([VisitID], [DocumentID], [DocumentType], [IpAddress],[Country], [Browser], [Device],[CreatedAt])
VALUES
(NEWID(), 'c1f62a41-2cb6-4d2c-8e98-2190b947c33e', 'Post', '192.40.57.235', 'Netherlands', 'Chrome', 'PC', GETDATE()),
(NEWID(), 'c1f62a41-2cb6-4d2c-8e98-2190b947c33e', 'Post', '188.130.155.151', 'Russian Federation', 'Chrome', 'Mobile', GETDATE()),
(NEWID(), 'c1f62a41-2cb6-4d2c-8e98-2190b947c33e', 'Post', '45.89.173.195', 'United States', 'Firefox', 'PC', GETDATE()),
(NEWID(), 'c1f62a41-2cb6-4d2c-8e98-2190b947c33e', 'Post', '190.2.138.14', 'Netherlands', 'Chrome', 'Mobile', GETDATE()),
(NEWID(), '57928261-c3f6-4cb4-baf6-194c067a8a93', 'Page', '2.18.219.255', 'United Kingdom', 'Safari', 'Mobile', GETDATE()),

(NEWID(), '57928261-c3f6-4cb4-baf6-194c067a8a93', 'Page', '102.132.114.255', 'Gambia', 'Firefox', 'PC', GETDATE()),
(NEWID(), '57928261-c3f6-4cb4-baf6-194c067a8a93', 'Page', '190.2.138.14', 'Netherlands', 'Chrome', 'Mobile', GETDATE()),
(NEWID(), '57928261-c3f6-4cb4-baf6-194c067a8a93', 'Page', '2.16.121.255', 'France', 'Internet Explorer', 'Mobile', GETDATE()),

(NEWID(), 'c1f62a41-2cb6-4d2c-8e98-2190b947c33e', 'Post', '45.89.173.195', 'United States', 'Firefox', 'PC', GETDATE()),
(NEWID(), 'c1f62a41-2cb6-4d2c-8e98-2190b947c33e', 'Post', '190.2.138.14', 'Netherlands', 'Chrome', 'Mobile', GETDATE()),
(NEWID(), '57928261-c3f6-4cb4-baf6-194c067a8a93', 'Page', '146.196.255.255', 'Gambia', 'Safari', 'Mobile', GETDATE()),

(NEWID(), '57928261-c3f6-4cb4-baf6-194c067a8a93', 'Page', '45.89.173.195', 'United States', 'Firefox', 'PC', GETDATE()),
(NEWID(), 'c1f62a41-2cb6-4d2c-8e98-2190b947c33e', 'Post', '190.2.138.14', 'Netherlands', 'Chrome', 'Mobile', GETDATE()),
(NEWID(), '57928261-c3f6-4cb4-baf6-194c067a8a93', 'Page', '196.223.144.0', 'Gambia', 'Safari', 'Mobile', GETDATE()),
(NEWID(), 'c1f62a41-2cb6-4d2c-8e98-2190b947c33e', 'Post', '89.187.171.228', 'United States', 'Internet Explorer', 'Mobile', GETDATE());



                                                                                                                                                                                                                      
/* ActivityLogs */
DROP TABLE IF EXISTS ActivityLogs 
CREATE TABLE [dbo].[ActivityLogs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ActivityID] [varchar](255) NOT NULL,
	[ActivityBy] [varchar](255) NULL,
	[ActivityType] [varchar](255) NULL,
	[Activity] [text] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[ActivityLogs] ADD CONSTRAINT unique_activity_id UNIQUE (ActivityID);
GO


                                                                                                                            

/* Subscribers */
DROP TABLE IF EXISTS Subscribers 
CREATE TABLE [dbo].[Subscribers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Email] [varchar](255) NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[Subscribers] ADD CONSTRAINT unique_subscriber_email UNIQUE (Email);
GO


/* SITE LANGUAGES */
DROP TABLE IF EXISTS SiteLanguages 
CREATE TABLE [dbo].[SiteLanguages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LanguageID] [varchar](250) NOT NULL,
	[Language] [varchar](250) NOT NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
) 
ALTER TABLE [dbo].[SiteLanguages] ADD CONSTRAINT unique_site_language_id UNIQUE ([LanguageID]);
GO

ALTER TABLE [dbo].[SiteLanguages] ADD CONSTRAINT unique_site_language_name UNIQUE ([Language]);
GO

INSERT INTO [SiteLanguages]
([LanguageID], [Language], [CreatedAt])
VALUES
(NEWID(), 'en', GETDATE());
-- (NEWID(), 'fr', GETDATE()),
-- (NEWID(), 'ar', GETDATE());


/* DATA TRANSLATIONS */
DROP TABLE IF EXISTS DataTranslations 
CREATE TABLE [dbo].[DataTranslations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DocumentID] [varchar](250) NOT NULL,/* id of the record being translated */
	[DocumentModel] [varchar](250) NOT NULL, /* document model */
	[Language] [varchar](250) NOT NULL, /* translatio language en, fr, ar... */
	[TranslationTitle] [nvarchar](250) NULL,
	[TranslationDescription] [nvarchar](500) NULL,
	[TranslationContent] [nvarchar](max) NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
) 
ALTER TABLE [dbo].[DataTranslations]
ADD CONSTRAINT unique_data_translations UNIQUE ([DocumentID], [DocumentModel], [Language]);
GO



/* PageSections */
DROP TABLE IF EXISTS PageSections 
CREATE TABLE [dbo].[PageSections](
    [ID] [int] IDENTITY(1,1) NOT NULL,
    [SectionID] [varchar](255) NOT NULL,
    [SectionName] [varchar](255) NOT NULL,
    [SectionTitle] [varchar](255) NOT NULL,
    [SectionContent] [text] NULL,
    [SectionType] [varchar](100) NOT NULL,
    [SectionOrder] [int] NOT NULL DEFAULT 0,
    [UpdatedAt] [datetime] NULL,
    [CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[PageSections] ADD CONSTRAINT unique_page_section_id UNIQUE (SectionID);
GO

INSERT INTO [PageSections]
([SectionID], [SectionName], [SectionTitle], [SectionType], [SectionOrder], [UpdatedAt])
VALUES
('4b9ddbce-fb7e-4baa-a2de-de2a4a2b7ff6', 'HomeSliders', 'Home Sliders', 'Default', '1', GETDATE()),
('4b1a8f9d-44b5-4673-9b26-b86108880e88', 'AboutUs', 'About Us', 'Default', '2', GETDATE()),
('0c2d5954-f6f9-4dfc-85df-2d7cc458f65c', 'SiteStats', 'Site Stats', 'Default', '3', GETDATE()),
('f32c5e9f-f7d5-4017-91d1-63e4e50f9eb5', 'HomeVideo', 'Home Video', 'Default', '4', GETDATE()),
('b1cc4547-a733-4c66-8787-2c816d0ee4d8', 'Services', 'Services', 'Default', '5', GETDATE()),
('10c8ac59-0885-4847-91b2-c3729a4fcc2d', 'Testimonials', 'Testimonials', 'Default', '6', GETDATE()),
('b28a66e5-98f4-4229-a7d0-856518a0b185', 'Posts', 'Posts', 'Default', '7', GETDATE()),
('e857569b-d1d9-4fb7-8c2f-4ff4d081db87', 'TwitterFeed', 'Twitter Feed', 'Default', '8', GETDATE()),
('b030c8e9-6177-4bdb-8294-9beaf743de4a', 'Donate', 'Donate', 'Default', '9', GETDATE()),
('644a032b-9e94-4426-bad3-ef8b40bbd0cc', 'Portfolio', 'Portfolio', 'Default', '10', GETDATE()),
('f0584cc9-1203-4cea-894f-d13172004118', 'OurTeam', 'Our Team', 'Default', '11', GETDATE()),
('bf93c7ba-0f22-4084-ad65-5c80ea1bcf01', 'FAQ', 'FAQ', 'Default', '12', GETDATE()),
('a02b8b4b-5927-4599-a1a9-4563aa07cdeb', 'Partners', 'Partners', 'Default', '13', GETDATE()),
('cfe8eede-f873-4a04-9bd5-1548a12f9f5c', 'ContactUs', 'Contact Us', 'Default', '14', GETDATE());




/* SectionManagement */
DROP TABLE IF EXISTS SectionManagement 
CREATE TABLE [dbo].[SectionManagement](
    [ID] [int] IDENTITY(1,1) NOT NULL,
    [SectionID] [varchar](255) NOT NULL,
    [SectionOrder] [varchar](255) NOT NULL,
    [UpdatedAt] [datetime] NULL,
    [CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[SectionManagement] ADD CONSTRAINT unique_section_id UNIQUE (SectionID);
GO

INSERT INTO [SectionManagement]
([SectionID], [SectionOrder],[UpdatedAt])
VALUES
('4b9ddbce-fb7e-4baa-a2de-de2a4a2b7ff6', '1', GETDATE()),
('4b1a8f9d-44b5-4673-9b26-b86108880e88', '2', GETDATE()),
('0c2d5954-f6f9-4dfc-85df-2d7cc458f65c', '3', GETDATE()),
('f32c5e9f-f7d5-4017-91d1-63e4e50f9eb5', '4', GETDATE()),
('b1cc4547-a733-4c66-8787-2c816d0ee4d8', '5', GETDATE()),
('10c8ac59-0885-4847-91b2-c3729a4fcc2d', '6', GETDATE()),
('b28a66e5-98f4-4229-a7d0-856518a0b185', '7', GETDATE()),
('644a032b-9e94-4426-bad3-ef8b40bbd0cc', '8', GETDATE()),
('f0584cc9-1203-4cea-894f-d13172004118', '9', GETDATE()),
('bf93c7ba-0f22-4084-ad65-5c80ea1bcf01', '10', GETDATE()),
('a02b8b4b-5927-4599-a1a9-4563aa07cdeb', '11', GETDATE()),
('cfe8eede-f873-4a04-9bd5-1548a12f9f5c', '12', GETDATE());



/* DonationCampaigns */
DROP TABLE IF EXISTS DonationCampaigns 
CREATE TABLE [dbo].[DonationCampaigns](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DonationID] [varchar](255) NOT NULL,
	[CampaignTitle] [varchar](255) NULL,
	[CampaignDescription] [text] NULL,
	[CampaignImage] [varchar](255) NULL,
	[DonationType] [varchar](100) NULL,
	[Link] [varchar](255) NOT NULL,
	[Status] [int] DEFAULT 0,
	[UpdatedAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL DEFAULT (getdate())
)
ALTER TABLE [dbo].[DonationCampaigns] ADD CONSTRAINT unique_donation_id UNIQUE (DonationID);
GO

INSERT INTO [DonationCampaigns]
([DonationID], [CampaignTitle], [CampaignDescription], [CampaignImage], [DonationType], [Link], [Status],[UpdatedAt])
VALUES
(NEWID(), 'We live to solve tough problems—and we don’t back down when things get hard. ', 'Post', 'campaign-image-1.jpg', 'Embed', 'https://donorbox.org/support-gexpotech', 1, GETDATE()),
(NEWID(), 'Build what the world needs next', 'Where donations to these emergency appeals meet the immediate needs of those affected we will use remaining funds for longer term relief to the communities affected by the emergency and then for other humanitarian relief within the relevant country in nearly all cases.', 'campaign-image-2.jpg', 'Embed', 'https://vancouverwe.com/donate-now', 1, GETDATE());


/* LANGUAGES */ 
DROP TABLE IF EXISTS Languages                                                                                                          
CREATE TABLE [dbo].[Languages](
	[ID] [int] NOT NULL PRIMARY KEY,
	[Name] [varchar](250) NOT NULL,
	[ISO] [varchar](2) NOT NULL
)
ALTER TABLE [dbo].[Languages] ADD CONSTRAINT unique_language_id UNIQUE ([ID]);
GO
INSERT INTO [Languages] VALUES(1, 'English', 'en');
INSERT INTO [Languages] VALUES(2, 'Afar', 'aa');
INSERT INTO [Languages] VALUES(3, 'Abkhazian', 'ab');
INSERT INTO [Languages] VALUES(4, 'Afrikaans', 'af');
INSERT INTO [Languages] VALUES(5, 'Amharic', 'am');
INSERT INTO [Languages] VALUES(6, 'Arabic', 'ar');
INSERT INTO [Languages] VALUES(7, 'Assamese', 'as');
INSERT INTO [Languages] VALUES(8, 'Aymara', 'ay');
INSERT INTO [Languages] VALUES(9, 'Azerbaijani', 'az');
INSERT INTO [Languages] VALUES(10, 'Bashkir', 'ba');
INSERT INTO [Languages] VALUES(11, 'Belarusian', 'be');
INSERT INTO [Languages] VALUES(12, 'Bulgarian', 'bg');
INSERT INTO [Languages] VALUES(13, 'Bihari', 'bh');
INSERT INTO [Languages] VALUES(14, 'Bislama', 'bi');
INSERT INTO [Languages] VALUES(15, 'Bengali/Bangla', 'bn');
INSERT INTO [Languages] VALUES(16, 'Tibetan', 'bo');
INSERT INTO [Languages] VALUES(17, 'Breton', 'br');
INSERT INTO [Languages] VALUES(18, 'Catalan', 'ca');
INSERT INTO [Languages] VALUES(19, 'Corsican', 'co');
INSERT INTO [Languages] VALUES(20, 'Czech', 'cs');
INSERT INTO [Languages] VALUES(21, 'Welsh', 'cy');
INSERT INTO [Languages] VALUES(22, 'Danish', 'da');
INSERT INTO [Languages] VALUES(23, 'German', 'de');
INSERT INTO [Languages] VALUES(24, 'Bhutani', 'dz');
INSERT INTO [Languages] VALUES(25, 'Greek', 'el');
INSERT INTO [Languages] VALUES(26, 'Esperanto', 'eo');
INSERT INTO [Languages] VALUES(27, 'Spanish', 'es');
INSERT INTO [Languages] VALUES(28, 'Estonian', 'et');
INSERT INTO [Languages] VALUES(29, 'Basque', 'eu');
INSERT INTO [Languages] VALUES(30, 'Persian', 'fa');
INSERT INTO [Languages] VALUES(31, 'Finnish', 'fi');
INSERT INTO [Languages] VALUES(32, 'Fiji', 'fj');
INSERT INTO [Languages] VALUES(33, 'Faeroese', 'fo');
INSERT INTO [Languages] VALUES(34, 'French', 'fr');
INSERT INTO [Languages] VALUES(35, 'Frisian', 'fy');
INSERT INTO [Languages] VALUES(36, 'Irish', 'ga');
INSERT INTO [Languages] VALUES(37, 'Scots/Gaelic', 'gd');
INSERT INTO [Languages] VALUES(38, 'Galician', 'gl');
INSERT INTO [Languages] VALUES(39, 'Guarani', 'gn');
INSERT INTO [Languages] VALUES(40, 'Gujarati', 'gu');
INSERT INTO [Languages] VALUES(41, 'Hausa', 'ha');
INSERT INTO [Languages] VALUES(42, 'Hindi', 'hi');
INSERT INTO [Languages] VALUES(43, 'Croatian', 'hr');
INSERT INTO [Languages] VALUES(44, 'Hungarian', 'hu');
INSERT INTO [Languages] VALUES(45, 'Armenian', 'hy');
INSERT INTO [Languages] VALUES(46, 'Interlingua', 'ia');
INSERT INTO [Languages] VALUES(47, 'Interlingue', 'ie');
INSERT INTO [Languages] VALUES(48, 'Inupiak', 'ik');
INSERT INTO [Languages] VALUES(49, 'Indonesian', 'in');
INSERT INTO [Languages] VALUES(50, 'Icelandic', 'is');
INSERT INTO [Languages] VALUES(51, 'Italian', 'it');
INSERT INTO [Languages] VALUES(52, 'Hebrew', 'iw');
INSERT INTO [Languages] VALUES(53, 'Japanese', 'ja');
INSERT INTO [Languages] VALUES(54, 'Yiddish', 'ji');
INSERT INTO [Languages] VALUES(55, 'Javanese', 'jw');
INSERT INTO [Languages] VALUES(56, 'Georgian', 'ka');
INSERT INTO [Languages] VALUES(57, 'Kazakh', 'kk');
INSERT INTO [Languages] VALUES(58, 'Greenlandic', 'kl');
INSERT INTO [Languages] VALUES(59, 'Cambodian', 'km');
INSERT INTO [Languages] VALUES(60, 'Kannada', 'kn');
INSERT INTO [Languages] VALUES(61, 'Korean', 'ko');
INSERT INTO [Languages] VALUES(62, 'Kashmiri', 'ks');
INSERT INTO [Languages] VALUES(63, 'Kurdish', 'ku');
INSERT INTO [Languages] VALUES(64, 'Kirghiz', 'ky');
INSERT INTO [Languages] VALUES(65, 'Latin', 'la');
INSERT INTO [Languages] VALUES(66, 'Lingala', 'ln');
INSERT INTO [Languages] VALUES(67, 'Laothian', 'lo');
INSERT INTO [Languages] VALUES(68, 'Lithuanian', 'lt');
INSERT INTO [Languages] VALUES(69, 'Latvian/Lettish', 'lv');
INSERT INTO [Languages] VALUES(70, 'Malagasy', 'mg');
INSERT INTO [Languages] VALUES(71, 'Maori', 'mi');
INSERT INTO [Languages] VALUES(72, 'Macedonian', 'mk');
INSERT INTO [Languages] VALUES(73, 'Malayalam', 'ml');
INSERT INTO [Languages] VALUES(74, 'Mongolian', 'mn');
INSERT INTO [Languages] VALUES(75, 'Moldavian', 'mo');
INSERT INTO [Languages] VALUES(76, 'Marathi', 'mr');
INSERT INTO [Languages] VALUES(77, 'Malay', 'ms');
INSERT INTO [Languages] VALUES(78, 'Maltese', 'mt');
INSERT INTO [Languages] VALUES(79, 'Burmese', 'my');
INSERT INTO [Languages] VALUES(80, 'Nauru', 'na');
INSERT INTO [Languages] VALUES(81, 'Nepali', 'ne');
INSERT INTO [Languages] VALUES(82, 'Dutch', 'nl');
INSERT INTO [Languages] VALUES(83, 'Norwegian', 'no');
INSERT INTO [Languages] VALUES(84, 'Occitan', 'oc');
INSERT INTO [Languages] VALUES(85, '(Afan)/Oromoor/Oriya', 'om');
INSERT INTO [Languages] VALUES(86, 'Punjabi', 'pa');
INSERT INTO [Languages] VALUES(87, 'Polish', 'pl');
INSERT INTO [Languages] VALUES(88, 'Pashto/Pushto', 'ps');
INSERT INTO [Languages] VALUES(89, 'Portuguese', 'pt');
INSERT INTO [Languages] VALUES(90, 'Quechua', 'qu');
INSERT INTO [Languages] VALUES(91, 'Rhaeto-Romance', 'rm');
INSERT INTO [Languages] VALUES(92, 'Kirundi', 'rn');
INSERT INTO [Languages] VALUES(93, 'Romanian', 'ro');
INSERT INTO [Languages] VALUES(94, 'Russian', 'ru');
INSERT INTO [Languages] VALUES(95, 'Kinyarwanda', 'rw');
INSERT INTO [Languages] VALUES(96, 'Sanskrit', 'sa');
INSERT INTO [Languages] VALUES(97, 'Sindhi', 'sd');
INSERT INTO [Languages] VALUES(98, 'Sangro', 'sg');
INSERT INTO [Languages] VALUES(99, 'Serbo-Croatian', 'sh');
INSERT INTO [Languages] VALUES(100, 'Singhalese', 'si');
INSERT INTO [Languages] VALUES(101, 'Slovak', 'sk');
INSERT INTO [Languages] VALUES(102, 'Slovenian', 'sl');
INSERT INTO [Languages] VALUES(103, 'Samoan', 'sm');
INSERT INTO [Languages] VALUES(104, 'Shona', 'sn');
INSERT INTO [Languages] VALUES(105, 'Somali', 'so');
INSERT INTO [Languages] VALUES(106, 'Albanian', 'sq');
INSERT INTO [Languages] VALUES(107, 'Serbian', 'sr');
INSERT INTO [Languages] VALUES(108, 'Siswati', 'ss');
INSERT INTO [Languages] VALUES(109, 'Sesotho', 'st');
INSERT INTO [Languages] VALUES(110, 'Sundanese', 'su');
INSERT INTO [Languages] VALUES(111, 'Swedish', 'sv');
INSERT INTO [Languages] VALUES(112, 'Swahili', 'sw');
INSERT INTO [Languages] VALUES(113, 'Tamil', 'ta');
INSERT INTO [Languages] VALUES(114, 'Telugu', 'te');
INSERT INTO [Languages] VALUES(115, 'Tajik', 'tg');
INSERT INTO [Languages] VALUES(116, 'Thai', 'th');
INSERT INTO [Languages] VALUES(117, 'Tigrinya', 'ti');
INSERT INTO [Languages] VALUES(118, 'Turkmen', 'tk');
INSERT INTO [Languages] VALUES(119, 'Tagalog', 'tl');
INSERT INTO [Languages] VALUES(120, 'Setswana', 'tn');
INSERT INTO [Languages] VALUES(121, 'Tonga', 'to');
INSERT INTO [Languages] VALUES(122, 'Turkish', 'tr');
INSERT INTO [Languages] VALUES(123, 'Tsonga', 'ts');
INSERT INTO [Languages] VALUES(124, 'Tatar', 'tt');
INSERT INTO [Languages] VALUES(125, 'Twi', 'tw');
INSERT INTO [Languages] VALUES(126, 'Ukrainian', 'uk');
INSERT INTO [Languages] VALUES(127, 'Urdu', 'ur');
INSERT INTO [Languages] VALUES(128, 'Uzbek', 'uz');
INSERT INTO [Languages] VALUES(129, 'Vietnamese', 'vi');
INSERT INTO [Languages] VALUES(130, 'Volapuk', 'vo');
INSERT INTO [Languages] VALUES(131, 'Wolof', 'wo');
INSERT INTO [Languages] VALUES(132, 'Xhosa', 'xh');
INSERT INTO [Languages] VALUES(133, 'Yoruba', 'yo');
INSERT INTO [Languages] VALUES(134, 'Chinese', 'zh');
INSERT INTO [Languages] VALUES(135, 'Zulu', 'zu');





/* COUNTRY */
DROP TABLE IF EXISTS Country  
CREATE TABLE [dbo].Country(
  ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
  ISO char(2) NOT NULL,
  Name varchar(80) NOT NULL,
  NiceName varchar(80) NOT NULL,
  ISO3 char(3) DEFAULT NULL,
  NumCode smallint DEFAULT NULL,
  PhoneCode int NOT NULL
)


SET IDENTITY_INSERT Country ON
INSERT INTO Country (ID, ISO, Name, Nicename, ISO3, NumCode, PhoneCode) VALUES
(1, 'AF', 'AFGHANISTAN', 'Afghanistan', 'AFG', 4, 93),
(2, 'AL', 'ALBANIA', 'Albania', 'ALB', 8, 355),
(3, 'DZ', 'ALGERIA', 'Algeria', 'DZA', 12, 213),
(4, 'AS', 'AMERICAN SAMOA', 'American Samoa', 'ASM', 16, 1684),
(5, 'AD', 'ANDORRA', 'Andorra', 'AND', 20, 376),
(6, 'AO', 'ANGOLA', 'Angola', 'AGO', 24, 244),
(7, 'AI', 'ANGUILLA', 'Anguilla', 'AIA', 660, 1264),
(8, 'AQ', 'ANTARCTICA', 'Antarctica', NULL, NULL, 0),
(9, 'AG', 'ANTIGUA AND BARBUDA', 'Antigua and Barbuda', 'ATG', 28, 1268),
(10, 'AR', 'ARGENTINA', 'Argentina', 'ARG', 32, 54),
(11, 'AM', 'ARMENIA', 'Armenia', 'ARM', 51, 374),
(12, 'AW', 'ARUBA', 'Aruba', 'ABW', 533, 297),
(13, 'AU', 'AUSTRALIA', 'Australia', 'AUS', 36, 61),
(14, 'AT', 'AUSTRIA', 'Austria', 'AUT', 40, 43),
(15, 'AZ', 'AZERBAIJAN', 'Azerbaijan', 'AZE', 31, 994),
(16, 'BS', 'BAHAMAS', 'Bahamas', 'BHS', 44, 1242),
(17, 'BH', 'BAHRAIN', 'Bahrain', 'BHR', 48, 973),
(18, 'BD', 'BANGLADESH', 'Bangladesh', 'BGD', 50, 880),
(19, 'BB', 'BARBADOS', 'Barbados', 'BRB', 52, 1246),
(20, 'BY', 'BELARUS', 'Belarus', 'BLR', 112, 375),
(21, 'BE', 'BELGIUM', 'Belgium', 'BEL', 56, 32),
(22, 'BZ', 'BELIZE', 'Belize', 'BLZ', 84, 501),
(23, 'BJ', 'BENIN', 'Benin', 'BEN', 204, 229),
(24, 'BM', 'BERMUDA', 'Bermuda', 'BMU', 60, 1441),
(25, 'BT', 'BHUTAN', 'Bhutan', 'BTN', 64, 975),
(26, 'BO', 'BOLIVIA', 'Bolivia', 'BOL', 68, 591),
(27, 'BA', 'BOSNIA AND HERZEGOVINA', 'Bosnia and Herzegovina', 'BIH', 70, 387),
(28, 'BW', 'BOTSWANA', 'Botswana', 'BWA', 72, 267),
(29, 'BV', 'BOUVET ISLAND', 'Bouvet Island', NULL, NULL, 0),
(30, 'BR', 'BRAZIL', 'Brazil', 'BRA', 76, 55),
(31, 'IO', 'BRITISH INDIAN OCEAN TERRITORY', 'British Indian Ocean Territory', NULL, NULL, 246),
(32, 'BN', 'BRUNEI DARUSSALAM', 'Brunei Darussalam', 'BRN', 96, 673),
(33, 'BG', 'BULGARIA', 'Bulgaria', 'BGR', 100, 359),
(34, 'BF', 'BURKINA FASO', 'Burkina Faso', 'BFA', 854, 226),
(35, 'BI', 'BURUNDI', 'Burundi', 'BDI', 108, 257),
(36, 'KH', 'CAMBODIA', 'Cambodia', 'KHM', 116, 855),
(37, 'CM', 'CAMEROON', 'Cameroon', 'CMR', 120, 237),
(38, 'CA', 'CANADA', 'Canada', 'CAN', 124, 1),
(39, 'CV', 'CAPE VERDE', 'Cape Verde', 'CPV', 132, 238),
(40, 'KY', 'CAYMAN ISLANDS', 'Cayman Islands', 'CYM', 136, 1345),
(41, 'CF', 'CENTRAL AFRICAN REPUBLIC', 'Central African Republic', 'CAF', 140, 236),
(42, 'TD', 'CHAD', 'Chad', 'TCD', 148, 235),
(43, 'CL', 'CHILE', 'Chile', 'CHL', 152, 56),
(44, 'CN', 'CHINA', 'China', 'CHN', 156, 86),
(45, 'CX', 'CHRISTMAS ISLAND', 'Christmas Island', NULL, NULL, 61),
(46, 'CC', 'COCOS (KEELING) ISLANDS', 'Cocos (Keeling) Islands', NULL, NULL, 672),
(47, 'CO', 'COLOMBIA', 'Colombia', 'COL', 170, 57),
(48, 'KM', 'COMOROS', 'Comoros', 'COM', 174, 269),
(49, 'CG', 'CONGO', 'Congo', 'COG', 178, 242),
(50, 'CD', 'CONGO, THE DEMOCRATIC REPUBLIC OF THE', 'Congo, the Democratic Republic of the', 'COD', 180, 242),
(51, 'CK', 'COOK ISLANDS', 'Cook Islands', 'COK', 184, 682),
(52, 'CR', 'COSTA RICA', 'Costa Rica', 'CRI', 188, 506),
(53, 'CI', 'COTE D''IVOIRE', 'Cote D''Ivoire', 'CIV', 384, 225),
(54, 'HR', 'CROATIA', 'Croatia', 'HRV', 191, 385),
(55, 'CU', 'CUBA', 'Cuba', 'CUB', 192, 53),
(56, 'CY', 'CYPRUS', 'Cyprus', 'CYP', 196, 357),
(57, 'CZ', 'CZECH REPUBLIC', 'Czech Republic', 'CZE', 203, 420),
(58, 'DK', 'DENMARK', 'Denmark', 'DNK', 208, 45),
(59, 'DJ', 'DJIBOUTI', 'Djibouti', 'DJI', 262, 253),
(60, 'DM', 'DOMINICA', 'Dominica', 'DMA', 212, 1767),
(61, 'DO', 'DOMINICAN REPUBLIC', 'Dominican Republic', 'DOM', 214, 1809),
(62, 'EC', 'ECUADOR', 'Ecuador', 'ECU', 218, 593),
(63, 'EG', 'EGYPT', 'Egypt', 'EGY', 818, 20),
(64, 'SV', 'EL SALVADOR', 'El Salvador', 'SLV', 222, 503),
(65, 'GQ', 'EQUATORIAL GUINEA', 'Equatorial Guinea', 'GNQ', 226, 240),
(66, 'ER', 'ERITREA', 'Eritrea', 'ERI', 232, 291),
(67, 'EE', 'ESTONIA', 'Estonia', 'EST', 233, 372),
(68, 'ET', 'ETHIOPIA', 'Ethiopia', 'ETH', 231, 251),
(69, 'FK', 'FALKLAND ISLANDS (MALVINAS)', 'Falkland Islands (Malvinas)', 'FLK', 238, 500),
(70, 'FO', 'FAROE ISLANDS', 'Faroe Islands', 'FRO', 234, 298),
(71, 'FJ', 'FIJI', 'Fiji', 'FJI', 242, 679),
(72, 'FI', 'FINLAND', 'Finland', 'FIN', 246, 358),
(73, 'FR', 'FRANCE', 'France', 'FRA', 250, 33),
(74, 'GF', 'FRENCH GUIANA', 'French Guiana', 'GUF', 254, 594),
(75, 'PF', 'FRENCH POLYNESIA', 'French Polynesia', 'PYF', 258, 689),
(76, 'TF', 'FRENCH SOUTHERN TERRITORIES', 'French Southern Territories', NULL, NULL, 0),
(77, 'GA', 'GABON', 'Gabon', 'GAB', 266, 241),
(78, 'GM', 'GAMBIA', 'Gambia', 'GMB', 270, 220),
(79, 'GE', 'GEORGIA', 'Georgia', 'GEO', 268, 995),
(80, 'DE', 'GERMANY', 'Germany', 'DEU', 276, 49),
(81, 'GH', 'GHANA', 'Ghana', 'GHA', 288, 233),
(82, 'GI', 'GIBRALTAR', 'Gibraltar', 'GIB', 292, 350),
(83, 'GR', 'GREECE', 'Greece', 'GRC', 300, 30),
(84, 'GL', 'GREENLAND', 'Greenland', 'GRL', 304, 299),
(85, 'GD', 'GRENADA', 'Grenada', 'GRD', 308, 1473),
(86, 'GP', 'GUADELOUPE', 'Guadeloupe', 'GLP', 312, 590),
(87, 'GU', 'GUAM', 'Guam', 'GUM', 316, 1671),
(88, 'GT', 'GUATEMALA', 'Guatemala', 'GTM', 320, 502),
(89, 'GN', 'GUINEA', 'Guinea', 'GIN', 324, 224),
(90, 'GW', 'GUINEA-BISSAU', 'Guinea-Bissau', 'GNB', 624, 245),
(91, 'GY', 'GUYANA', 'Guyana', 'GUY', 328, 592),
(92, 'HT', 'HAITI', 'Haiti', 'HTI', 332, 509),
(93, 'HM', 'HEARD ISLAND AND MCDONALD ISLANDS', 'Heard Island and Mcdonald Islands', NULL, NULL, 0),
(94, 'VA', 'HOLY SEE (VATICAN CITY STATE)', 'Holy See (Vatican City State)', 'VAT', 336, 39),
(95, 'HN', 'HONDURAS', 'Honduras', 'HND', 340, 504),
(96, 'HK', 'HONG KONG', 'Hong Kong', 'HKG', 344, 852),
(97, 'HU', 'HUNGARY', 'Hungary', 'HUN', 348, 36),
(98, 'IS', 'ICELAND', 'Iceland', 'ISL', 352, 354),
(99, 'IN', 'INDIA', 'India', 'IND', 356, 91),
(100, 'ID', 'INDONESIA', 'Indonesia', 'IDN', 360, 62),
(101, 'IR', 'IRAN, ISLAMIC REPUBLIC OF', 'Iran, Islamic Republic of', 'IRN', 364, 98),
(102, 'IQ', 'IRAQ', 'Iraq', 'IRQ', 368, 964),
(103, 'IE', 'IRELAND', 'Ireland', 'IRL', 372, 353),
(104, 'IL', 'ISRAEL', 'Israel', 'ISR', 376, 972),
(105, 'IT', 'ITALY', 'Italy', 'ITA', 380, 39),
(106, 'JM', 'JAMAICA', 'Jamaica', 'JAM', 388, 1876),
(107, 'JP', 'JAPAN', 'Japan', 'JPN', 392, 81),
(108, 'JO', 'JORDAN', 'Jordan', 'JOR', 400, 962),
(109, 'KZ', 'KAZAKHSTAN', 'Kazakhstan', 'KAZ', 398, 7),
(110, 'KE', 'KENYA', 'Kenya', 'KEN', 404, 254),
(111, 'KI', 'KIRIBATI', 'Kiribati', 'KIR', 296, 686),
(112, 'KP', 'KOREA, DEMOCRATIC PEOPLE''S REPUBLIC OF', 'Korea, Democratic People''s Republic of', 'PRK', 408, 850),
(113, 'KR', 'KOREA, REPUBLIC OF', 'Korea, Republic of', 'KOR', 410, 82),
(114, 'KW', 'KUWAIT', 'Kuwait', 'KWT', 414, 965),
(115, 'KG', 'KYRGYZSTAN', 'Kyrgyzstan', 'KGZ', 417, 996),
(116, 'LA', 'LAO PEOPLE''S DEMOCRATIC REPUBLIC', 'Lao People''s Democratic Republic', 'LAO', 418, 856),
(117, 'LV', 'LATVIA', 'Latvia', 'LVA', 428, 371),
(118, 'LB', 'LEBANON', 'Lebanon', 'LBN', 422, 961),
(119, 'LS', 'LESOTHO', 'Lesotho', 'LSO', 426, 266),
(120, 'LR', 'LIBERIA', 'Liberia', 'LBR', 430, 231),
(121, 'LY', 'LIBYAN ARAB JAMAHIRIYA', 'Libyan Arab Jamahiriya', 'LBY', 434, 218),
(122, 'LI', 'LIECHTENSTEIN', 'Liechtenstein', 'LIE', 438, 423),
(123, 'LT', 'LITHUANIA', 'Lithuania', 'LTU', 440, 370),
(124, 'LU', 'LUXEMBOURG', 'Luxembourg', 'LUX', 442, 352),
(125, 'MO', 'MACAO', 'Macao', 'MAC', 446, 853),
(126, 'MK', 'MACEDONIA, THE FORMER YUGOSLAV REPUBLIC OF', 'Macedonia, the Former Yugoslav Republic of', 'MKD', 807, 389),
(127, 'MG', 'MADAGASCAR', 'Madagascar', 'MDG', 450, 261),
(128, 'MW', 'MALAWI', 'Malawi', 'MWI', 454, 265),
(129, 'MY', 'MALAYSIA', 'Malaysia', 'MYS', 458, 60),
(130, 'MV', 'MALDIVES', 'Maldives', 'MDV', 462, 960),
(131, 'ML', 'MALI', 'Mali', 'MLI', 466, 223),
(132, 'MT', 'MALTA', 'Malta', 'MLT', 470, 356),
(133, 'MH', 'MARSHALL ISLANDS', 'Marshall Islands', 'MHL', 584, 692),
(134, 'MQ', 'MARTINIQUE', 'Martinique', 'MTQ', 474, 596),
(135, 'MR', 'MAURITANIA', 'Mauritania', 'MRT', 478, 222),
(136, 'MU', 'MAURITIUS', 'Mauritius', 'MUS', 480, 230),
(137, 'YT', 'MAYOTTE', 'Mayotte', NULL, NULL, 269),
(138, 'MX', 'MEXICO', 'Mexico', 'MEX', 484, 52),
(139, 'FM', 'MICRONESIA, FEDERATED STATES OF', 'Micronesia, Federated States of', 'FSM', 583, 691),
(140, 'MD', 'MOLDOVA, REPUBLIC OF', 'Moldova, Republic of', 'MDA', 498, 373),
(141, 'MC', 'MONACO', 'Monaco', 'MCO', 492, 377),
(142, 'MN', 'MONGOLIA', 'Mongolia', 'MNG', 496, 976),
(143, 'MS', 'MONTSERRAT', 'Montserrat', 'MSR', 500, 1664),
(144, 'MA', 'MOROCCO', 'Morocco', 'MAR', 504, 212),
(145, 'MZ', 'MOZAMBIQUE', 'Mozambique', 'MOZ', 508, 258),
(146, 'MM', 'MYANMAR', 'Myanmar', 'MMR', 104, 95),
(147, 'NA', 'NAMIBIA', 'Namibia', 'NAM', 516, 264),
(148, 'NR', 'NAURU', 'Nauru', 'NRU', 520, 674),
(149, 'NP', 'NEPAL', 'Nepal', 'NPL', 524, 977),
(150, 'NL', 'NETHERLANDS', 'Netherlands', 'NLD', 528, 31),
(151, 'AN', 'NETHERLANDS ANTILLES', 'Netherlands Antilles', 'ANT', 530, 599),
(152, 'NC', 'NEW CALEDONIA', 'New Caledonia', 'NCL', 540, 687),
(153, 'NZ', 'NEW ZEALAND', 'New Zealand', 'NZL', 554, 64),
(154, 'NI', 'NICARAGUA', 'Nicaragua', 'NIC', 558, 505),
(155, 'NE', 'NIGER', 'Niger', 'NER', 562, 227),
(156, 'NG', 'NIGERIA', 'Nigeria', 'NGA', 566, 234),
(157, 'NU', 'NIUE', 'Niue', 'NIU', 570, 683),
(158, 'NF', 'NORFOLK ISLAND', 'Norfolk Island', 'NFK', 574, 672),
(159, 'MP', 'NORTHERN MARIANA ISLANDS', 'Northern Mariana Islands', 'MNP', 580, 1670),
(160, 'NO', 'NORWAY', 'Norway', 'NOR', 578, 47),
(161, 'OM', 'OMAN', 'Oman', 'OMN', 512, 968),
(162, 'PK', 'PAKISTAN', 'Pakistan', 'PAK', 586, 92),
(163, 'PW', 'PALAU', 'Palau', 'PLW', 585, 680),
(164, 'PS', 'PALESTINIAN TERRITORY, OCCUPIED', 'Palestinian Territory, Occupied', NULL, NULL, 970),
(165, 'PA', 'PANAMA', 'Panama', 'PAN', 591, 507),
(166, 'PG', 'PAPUA NEW GUINEA', 'Papua New Guinea', 'PNG', 598, 675),
(167, 'PY', 'PARAGUAY', 'Paraguay', 'PRY', 600, 595),
(168, 'PE', 'PERU', 'Peru', 'PER', 604, 51),
(169, 'PH', 'PHILIPPINES', 'Philippines', 'PHL', 608, 63),
(170, 'PN', 'PITCAIRN', 'Pitcairn', 'PCN', 612, 0),
(171, 'PL', 'POLAND', 'Poland', 'POL', 616, 48),
(172, 'PT', 'PORTUGAL', 'Portugal', 'PRT', 620, 351),
(173, 'PR', 'PUERTO RICO', 'Puerto Rico', 'PRI', 630, 1787),
(174, 'QA', 'QATAR', 'Qatar', 'QAT', 634, 974),
(175, 'RE', 'REUNION', 'Reunion', 'REU', 638, 262),
(176, 'RO', 'ROMANIA', 'Romania', 'ROM', 642, 40),
(177, 'RU', 'RUSSIAN FEDERATION', 'Russian Federation', 'RUS', 643, 70),
(178, 'RW', 'RWANDA', 'Rwanda', 'RWA', 646, 250),
(179, 'SH', 'SAINT HELENA', 'Saint Helena', 'SHN', 654, 290),
(180, 'KN', 'SAINT KITTS AND NEVIS', 'Saint Kitts and Nevis', 'KNA', 659, 1869),
(181, 'LC', 'SAINT LUCIA', 'Saint Lucia', 'LCA', 662, 1758),
(182, 'PM', 'SAINT PIERRE AND MIQUELON', 'Saint Pierre and Miquelon', 'SPM', 666, 508),
(183, 'VC', 'SAINT VINCENT AND THE GRENADINES', 'Saint Vincent and the Grenadines', 'VCT', 670, 1784),
(184, 'WS', 'SAMOA', 'Samoa', 'WSM', 882, 684),
(185, 'SM', 'SAN MARINO', 'San Marino', 'SMR', 674, 378),
(186, 'ST', 'SAO TOME AND PRINCIPE', 'Sao Tome and Principe', 'STP', 678, 239),
(187, 'SA', 'SAUDI ARABIA', 'Saudi Arabia', 'SAU', 682, 966),
(188, 'SN', 'SENEGAL', 'Senegal', 'SEN', 686, 221),
(189, 'CS', 'SERBIA AND MONTENEGRO', 'Serbia and Montenegro', NULL, NULL, 381),
(190, 'SC', 'SEYCHELLES', 'Seychelles', 'SYC', 690, 248),
(191, 'SL', 'SIERRA LEONE', 'Sierra Leone', 'SLE', 694, 232),
(192, 'SG', 'SINGAPORE', 'Singapore', 'SGP', 702, 65),
(193, 'SK', 'SLOVAKIA', 'Slovakia', 'SVK', 703, 421),
(194, 'SI', 'SLOVENIA', 'Slovenia', 'SVN', 705, 386),
(195, 'SB', 'SOLOMON ISLANDS', 'Solomon Islands', 'SLB', 90, 677),
(196, 'SO', 'SOMALIA', 'Somalia', 'SOM', 706, 252),
(197, 'ZA', 'SOUTH AFRICA', 'South Africa', 'ZAF', 710, 27),
(198, 'GS', 'SOUTH GEORGIA AND THE SOUTH SANDWICH ISLANDS', 'South Georgia and the South Sandwich Islands', NULL, NULL, 0),
(199, 'ES', 'SPAIN', 'Spain', 'ESP', 724, 34),
(200, 'LK', 'SRI LANKA', 'Sri Lanka', 'LKA', 144, 94),
(201, 'SD', 'SUDAN', 'Sudan', 'SDN', 736, 249),
(202, 'SR', 'SURINAME', 'Suriname', 'SUR', 740, 597),
(203, 'SJ', 'SVALBARD AND JAN MAYEN', 'Svalbard and Jan Mayen', 'SJM', 744, 47),
(204, 'SZ', 'SWAZILAND', 'Swaziland', 'SWZ', 748, 268),
(205, 'SE', 'SWEDEN', 'Sweden', 'SWE', 752, 46),
(206, 'CH', 'SWITZERLAND', 'Switzerland', 'CHE', 756, 41),
(207, 'SY', 'SYRIAN ARAB REPUBLIC', 'Syrian Arab Republic', 'SYR', 760, 963),
(208, 'TW', 'TAIWAN, PROVINCE OF CHINA', 'Taiwan, Province of China', 'TWN', 158, 886),
(209, 'TJ', 'TAJIKISTAN', 'Tajikistan', 'TJK', 762, 992),
(210, 'TZ', 'TANZANIA, UNITED REPUBLIC OF', 'Tanzania, United Republic of', 'TZA', 834, 255),
(211, 'TH', 'THAILAND', 'Thailand', 'THA', 764, 66),
(212, 'TL', 'TIMOR-LESTE', 'Timor-Leste', NULL, NULL, 670),
(213, 'TG', 'TOGO', 'Togo', 'TGO', 768, 228),
(214, 'TK', 'TOKELAU', 'Tokelau', 'TKL', 772, 690),
(215, 'TO', 'TONGA', 'Tonga', 'TON', 776, 676),
(216, 'TT', 'TRINIDAD AND TOBAGO', 'Trinidad and Tobago', 'TTO', 780, 1868),
(217, 'TN', 'TUNISIA', 'Tunisia', 'TUN', 788, 216),
(218, 'TR', 'TURKEY', 'Turkey', 'TUR', 792, 90),
(219, 'TM', 'TURKMENISTAN', 'Turkmenistan', 'TKM', 795, 7370),
(220, 'TC', 'TURKS AND CAICOS ISLANDS', 'Turks and Caicos Islands', 'TCA', 796, 1649),
(221, 'TV', 'TUVALU', 'Tuvalu', 'TUV', 798, 688),
(222, 'UG', 'UGANDA', 'Uganda', 'UGA', 800, 256),
(223, 'UA', 'UKRAINE', 'Ukraine', 'UKR', 804, 380),
(224, 'AE', 'UNITED ARAB EMIRATES', 'United Arab Emirates', 'ARE', 784, 971),
(225, 'GB', 'UNITED KINGDOM', 'United Kingdom', 'GBR', 826, 44),
(226, 'US', 'UNITED STATES', 'United States', 'USA', 840, 1),
(227, 'UM', 'UNITED STATES MINOR OUTLYING ISLANDS', 'United States Minor Outlying Islands', NULL, NULL, 1),
(228, 'UY', 'URUGUAY', 'Uruguay', 'URY', 858, 598),
(229, 'UZ', 'UZBEKISTAN', 'Uzbekistan', 'UZB', 860, 998),
(230, 'VU', 'VANUATU', 'Vanuatu', 'VUT', 548, 678),
(231, 'VE', 'VENEZUELA', 'Venezuela', 'VEN', 862, 58),
(232, 'VN', 'VIET NAM', 'Viet Nam', 'VNM', 704, 84),
(233, 'VG', 'VIRGIN ISLANDS, BRITISH', 'Virgin Islands, British', 'VGB', 92, 1284),
(234, 'VI', 'VIRGIN ISLANDS, U.S.', 'Virgin Islands, U.s.', 'VIR', 850, 1340),
(235, 'WF', 'WALLIS AND FUTUNA', 'Wallis and Futuna', 'WLF', 876, 681),
(236, 'EH', 'WESTERN SAHARA', 'Western Sahara', 'ESH', 732, 212),
(237, 'YE', 'YEMEN', 'Yemen', 'YEM', 887, 967),
(238, 'ZM', 'ZAMBIA', 'Zambia', 'ZMB', 894, 260),
(239, 'ZW', 'ZIMBABWE', 'Zimbabwe', 'ZWE', 716, 263),
(240, 'RS', 'SERBIA', 'Serbia', 'SRB', 688, 381),
(241, 'AP', 'ASIA PACIFIC REGION', 'Asia / Pacific Region', '0', 0, 0),
(242, 'ME', 'MONTENEGRO', 'Montenegro', 'MNE', 499, 382),
(243, 'AX', 'ALAND ISLANDS', 'Aland Islands', 'ALA', 248, 358),
(244, 'BQ', 'BONAIRE, SINT EUSTATIUS AND SABA', 'Bonaire, Sint Eustatius and Saba', 'BES', 535, 599),
(245, 'CW', 'CURACAO', 'Curacao', 'CUW', 531, 599),
(246, 'GG', 'GUERNSEY', 'Guernsey', 'GGY', 831, 44),
(247, 'IM', 'ISLE OF MAN', 'Isle of Man', 'IMN', 833, 44),
(248, 'JE', 'JERSEY', 'Jersey', 'JEY', 832, 44),
(249, 'XK', 'KOSOVO', 'Kosovo', '---', 0, 381),
(250, 'BL', 'SAINT BARTHELEMY', 'Saint Barthelemy', 'BLM', 652, 590),
(251, 'MF', 'SAINT MARTIN', 'Saint Martin', 'MAF', 663, 590),
(252, 'SX', 'SINT MAARTEN', 'Sint Maarten', 'SXM', 534, 1),
(253, 'SS', 'SOUTH SUDAN', 'South Sudan', 'SSD', 728, 211)

SET IDENTITY_INSERT Country OFF
GO



--VIEWS
CREATE OR ALTER VIEW [PopularThisWeek]
AS
SELECT TOP (50) DocumentID, COUNT(DocumentID) AS ValueOccurrence
FROM            dbo.SiteVisits
WHERE        (CreatedAt >= GETDATE() - 7) AND (CreatedAt < GETDATE()) AND (DocumentID IN
             (SELECT PostID FROM dbo.Posts))
GROUP BY DocumentID
ORDER BY 'ValueOccurrence' DESC