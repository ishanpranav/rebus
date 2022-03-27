BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "dotnet_migration" (
	"MigrationId"	TEXT NOT NULL,
	"ProductVersion"	TEXT NOT NULL,
	CONSTRAINT "PK_dotnet_migration" PRIMARY KEY("MigrationId")
);
CREATE TABLE IF NOT EXISTS "Token" (
	"Value"	TEXT NOT NULL COLLATE NOCASE,
	"Type"	INTEGER NOT NULL,
	CONSTRAINT "PK_Token" PRIMARY KEY("Value")
);
CREATE TABLE IF NOT EXISTS "Adjective" (
	"TokenValue"	TEXT NOT NULL,
	"ConceptId"	INTEGER NOT NULL,
	CONSTRAINT "FK_Adjective_Concept_ConceptId" FOREIGN KEY("ConceptId") REFERENCES "Concept"("Id") ON DELETE CASCADE,
	CONSTRAINT "FK_Adjective_Token_TokenValue" FOREIGN KEY("TokenValue") REFERENCES "Token"("Value") ON DELETE CASCADE,
	CONSTRAINT "PK_Adjective" PRIMARY KEY("TokenValue","ConceptId")
);
CREATE TABLE IF NOT EXISTS "Command" (
	"Id"	INTEGER NOT NULL,
	"Guid"	TEXT NOT NULL,
	"VerbValue"	TEXT NOT NULL,
	"AdverbValue"	TEXT,
	CONSTRAINT "FK_Command_Token_AdverbValue" FOREIGN KEY("AdverbValue") REFERENCES "Token"("Value"),
	CONSTRAINT "FK_Command_Token_VerbValue" FOREIGN KEY("VerbValue") REFERENCES "Token"("Value") ON DELETE CASCADE,
	CONSTRAINT "PK_Command" PRIMARY KEY("Id" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "Navigation" (
	"Id"	INTEGER NOT NULL,
	"PlayerId"	INTEGER NOT NULL,
	"Q"	INTEGER NOT NULL,
	"R"	INTEGER NOT NULL,
	CONSTRAINT "FK_Navigation_Player_PlayerId" FOREIGN KEY("PlayerId") REFERENCES "Player"("Id") ON DELETE CASCADE,
	CONSTRAINT "PK_Navigation" PRIMARY KEY("Id" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "Planet" (
	"Id"	INTEGER NOT NULL,
	"Q"	INTEGER NOT NULL,
	"R"	INTEGER NOT NULL,
	CONSTRAINT "FK_Planet_Concept_Id" FOREIGN KEY("Id") REFERENCES "Concept"("Id") ON DELETE CASCADE,
	CONSTRAINT "PK_Planet" PRIMARY KEY("Id" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "Spacecraft" (
	"Id"	INTEGER NOT NULL,
	"Q"	INTEGER NOT NULL,
	"R"	INTEGER NOT NULL,
	"PlayerId"	INTEGER NOT NULL,
	CONSTRAINT "FK_Spacecraft_Concept_Id" FOREIGN KEY("Id") REFERENCES "Concept"("Id") ON DELETE CASCADE,
	CONSTRAINT "FK_Spacecraft_Player_PlayerId" FOREIGN KEY("PlayerId") REFERENCES "Player"("Id") ON DELETE CASCADE,
	CONSTRAINT "PK_Spacecraft" PRIMARY KEY("Id" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "Star" (
	"Id"	INTEGER NOT NULL,
	"Q"	INTEGER NOT NULL,
	"R"	INTEGER NOT NULL,
	CONSTRAINT "FK_Star_Concept_Id" FOREIGN KEY("Id") REFERENCES "Concept"("Id") ON DELETE CASCADE,
	CONSTRAINT "PK_Star" PRIMARY KEY("Id" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "Concept" (
	"Id"	INTEGER NOT NULL,
	"ArticleValue"	TEXT,
	"SubstantiveValue"	TEXT NOT NULL,
	CONSTRAINT "FK_Concept_Token_SubstantiveValue" FOREIGN KEY("SubstantiveValue") REFERENCES "Token"("Value") ON DELETE CASCADE,
	CONSTRAINT "FK_Concept_Token_ArticleValue" FOREIGN KEY("ArticleValue") REFERENCES "Token"("Value"),
	CONSTRAINT "PK_Concept" PRIMARY KEY("Id" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "Resource" (
	"Id"	INTEGER NOT NULL,
	"Arguments"	INTEGER NOT NULL,
	"Key"	TEXT NOT NULL DEFAULT '' COLLATE NOCASE,
	"Value"	TEXT NOT NULL DEFAULT '',
	CONSTRAINT "PK_Resource" PRIMARY KEY("Id" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "Player" (
	"Id"	INTEGER NOT NULL,
	"Credential"	TEXT NOT NULL DEFAULT '' COLLATE NOCASE,
	"Sequence"	INTEGER NOT NULL DEFAULT 1,
	"UserId"	TEXT NOT NULL DEFAULT '' COLLATE NOCASE,
	"Wealth"	INTEGER NOT NULL,
	CONSTRAINT "PK_Player" PRIMARY KEY("Id" AUTOINCREMENT)
);
INSERT INTO "dotnet_migration" VALUES ('20220322182304_InitialMigration','6.0.3'),
 ('20220326052725_NavigationMigration','6.0.3');
INSERT INTO "Token" VALUES ('redo',1048576),
 ('reexecute',1048576),
 ('repeat',1048576),
 ('undo',1048576),
 ('look',1048576),
 ('survey',1048576),
 ('report',1048576),
 ('observe',1048576),
 ('around',2048),
 ('view',1048576),
 ('please',8192),
 ('ok',8192),
 ('I',32768),
 ('G',1572864),
 ('an',1),
 ('a',1),
 ('hello',8192),
 ('me',49152),
 ('myself',49152),
 ('self',147456),
 ('that',2),
 ('the',3),
 ('those',2),
 ('us',16384),
 ('we',32768),
 ('you',180224),
 ('yourself',180224),
 ('my',2),
 ('or',4096),
 ('and',4096),
 ('rename',1048576),
 ('name',1048576),
 ('navigate',1048576),
 ('go',1048576),
 ('fly',1048576),
 ('to',2048),
 ('travel',1048576),
 ('journey',1048576),
 ('explore',1048576),
 ('visit',1048576),
 ('return',1048576),
 ('enter',1048576),
 ('in',2048),
 ('into',2048),
 ('venture',1048576),
 ('spacecraft',262144),
 ('move',1048576),
 ('goto',1048576),
 ('pilot',1048576),
 ('autopilot',1048576),
 ('budget',1048576),
 ('balance',1048576),
 ('bank',1048576);
INSERT INTO "Command" VALUES (1,'5A0EE5FA-D968-49A2-AA29-8131A6B39AA9','redo',NULL),
 (2,'AD7EF788-9CE6-4EA0-9A5B-7C3CFF1FB146','reexecute',NULL),
 (3,'AD7EF788-9CE6-4EA0-9A5B-7C3CFF1FB146','repeat',NULL),
 (4,'86EEE5CE-5FFF-4420-9B3F-5A4DC96B54A0','undo',NULL),
 (5,'45DA897D-68E8-47D5-A946-76FDE16B826B','rename',NULL),
 (6,'45DA897D-68E8-47D5-A946-76FDE16B826B','name',NULL),
 (7,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','navigate','to'),
 (8,'6C4E330D-29AD-498B-B6A9-CB45724B0A32','look',NULL),
 (9,'6C4E330D-29AD-498B-B6A9-CB45724B0A32','survey',NULL),
 (10,'6C4E330D-29AD-498B-B6A9-CB45724B0A32','observe',NULL),
 (11,'6C4E330D-29AD-498B-B6A9-CB45724B0A32','look','around'),
 (12,'6C4E330D-29AD-498B-B6A9-CB45724B0A32','view',NULL),
 (13,'6C4E330D-29AD-498B-B6A9-CB45724B0A32','view','around'),
 (14,'AD7EF788-9CE6-4EA0-9A5B-7C3CFF1FB146','G',NULL),
 (15,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','fly','to'),
 (16,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','travel','to'),
 (17,'6C4E330D-29AD-498B-B6A9-CB45724B0A32','report',NULL),
 (18,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','journey','to'),
 (19,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','explore',NULL),
 (20,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','return','to'),
 (21,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','enter',NULL),
 (22,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','visit',NULL),
 (23,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','go','to'),
 (24,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','go','in'),
 (25,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','venture','into'),
 (26,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','go','into'),
 (27,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','travel','into'),
 (28,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','fly','into'),
 (29,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','move','to'),
 (30,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','move','into'),
 (31,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','goto',NULL),
 (32,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','pilot','to'),
 (33,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','autopilot','to'),
 (34,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','autopilot','into'),
 (35,'58D34F6D-B48D-49BC-88D7-0777E9039ABF','pilot','into'),
 (36,'07BFE879-ABE2-41CA-9A8F-2337E8DEB668','balance',NULL),
 (37,'07BFE879-ABE2-41CA-9A8F-2337E8DEB668','bank',NULL),
 (38,'07BFE879-ABE2-41CA-9A8F-2337E8DEB668','budget',NULL);
INSERT INTO "Resource" VALUES (1,0,'UnexpectedParsingError','sorry,I did not understand the last part of that sentence.'),
 (7,1,'ExpectedParsingError','sorry,I do not know that {0:or}.'),
 (8,0,'EmptyParsingError','please enter an imperative sentence.'),
 (9,0,'EmptyParsingError','please enter an instruction.'),
 (10,0,'EmptyParsingError','what is your command?'),
 (11,0,'EmptyParsingError','what is your instruction?'),
 (12,0,'EmptyParsingError','what are your instructions?'),
 (13,0,'EmptyParsingError','please enter a command.'),
 (14,0,'EmptyParsingError','what is your order?'),
 (15,0,'EmptyParsingError','what do you want us to do?'),
 (16,0,'EmptyParsingError','i await your orders.'),
 (17,1,'ExpectedParsingError','sorry,I am not familiar with that {0:or}.'),
 (18,2,'ExpectedParsingError','sorry,I do not know that {0:or}.did you mean {1:or}?'),
 (19,2,'ExpectedParsingError','sorry,I am not familiar with that {0:or}.did you mean {1:or}?'),
 (20,1,'ExpectedParsingError','sorry,I was expecting a {0:or}.'),
 (21,2,'ExpectedParsingError','sorry,I was expecting a {0:or}.did you mean {1:or}?'),
 (24,0,'UndefinedConcept','i do not know about anything that satisfies that description.'),
 (25,0,'UndefinedConcept','i do not know what that is.'),
 (26,0,'AmbiguousConcept','i know about multiple ways to do that.'),
 (27,0,'UndefinedCommand','i do not know how to do that.'),
 (28,1,'ExpectedParsingError','sorry,I expected a {0:or}.'),
 (29,2,'ExpectedParsingError','sorry,I expected a {0:or}.did you mean {1:or}?'),
 (30,1,'UnexpectedParsingError','did you mean {0:or}?'),
 (31,1,'UnexpectedParsingError','did you mean {0:or perhaps}?'),
 (33,1,'FirstWelcome','welcome,{0}.please enter a credential to access your account in the future.'),
 (34,4,'Header','ishan Pranav''s REBUS.copyright (c) 2021-2022 Ishan Pranav.all rights reserved.this software is licensed with the MIT license.version {0}.{1}.{2} for {3}.greetings,I am REBUS,your microcomputer assistant.with my instruments and your intellect,we will command vast fleets of spacecraft and explore the galaxy.i was created at Arnold O.beckman High School in Irvine,California by Ishan Pranav using the C# programming language.'),
 (38,1,'SecondWelcome','welcome back,{0}.what is your credential?'),
 (39,0,'EmptyParsingError','our forces await your instructions.'),
 (40,0,'CredentialFailure','sorry,that credential is invalid.'),
 (41,1,'RegionDescription','we are in an empty region near {0:and}.'),
 (42,2,'RegionDescription','we are just outside {0}''s atmosphere.{1:and} neighbor this region.'),
 (43,2,'RegionDescription','we are orbiting {0},which neighbors {1:and}.'),
 (44,2,'StarDescription','the light of the star {0} shines from the {1} direction.'),
 (45,2,'StarDescription','{0}''s aura emanates from the {1} direction.'),
 (46,2,'TransmissionHeader','incoming transmission from {0:and} in {1}:'),
 (47,2,'RegionDescription','we are currently in {0}''s orbit.{1:and} are nearby.'),
 (52,0,'EmptyParsingError','our spacecraft are prepared to receive your orders.'),
 (53,0,'RedoSuccess','redone.'),
 (54,0,'UndoSuccess','undone.'),
 (55,0,'UndoFailure','i cannot undo the previous action.'),
 (56,0,'RedoFailure','there is nothing for me to redo.'),
 (57,0,'ReexecuteFailure','there is nothing for me to repeat.'),
 (58,0,'ReexecuteSuccess','repeated.'),
 (59,0,'UndoFailure','there is nothing that I can undo.'),
 (60,0,'UndoFailure','there is nothing for me to undo.'),
 (61,0,'ReexecuteFailure','there is nothing that I can repeat.'),
 (62,0,'RedoFailure','there is nothing that I can redo.'),
 (63,0,'UndoFailure','i cannot undo anything right now.'),
 (64,0,'ReexecuteFailure','i cannot repeat anything right now.'),
 (65,0,'RedoFailure','i cannot redo anything right now.'),
 (66,0,'NavigationFailure','we are already here.'),
 (67,0,'NavigationFailure','we are already in that sector.'),
 (68,0,'EmptyParsingError','we are ready for your orders.'),
 (69,0,'EmptyParsingError','what are your orders?'),
 (70,2,'TransmissionHeader','transmission from {0:and} in {1}:'),
 (71,2,'TransmissionHeader','message from {0:and} in {1}:'),
 (72,2,'TransmissionHeader','{0:and} in {1}:'),
 (73,1,'WealthIncrease','wealth has increased by {0} {0:credit,credits}.'),
 (74,2,'WealthPenalty','we have returned {0:p0} interest ({1} {1:credit,credits}) to our creditors.'),
 (75,1,'WealthDecrease','wealth has decreased by {0} {0:credit,credits}.'),
 (76,1,'WealthBalance','we have {0} {0:credit,credits}.');
CREATE INDEX IF NOT EXISTS "IX_Adjective_ConceptId" ON "Adjective" (
	"ConceptId"
);
CREATE INDEX IF NOT EXISTS "IX_Command_AdverbValue" ON "Command" (
	"AdverbValue"
);
CREATE INDEX IF NOT EXISTS "IX_Command_VerbValue" ON "Command" (
	"VerbValue"
);
CREATE UNIQUE INDEX IF NOT EXISTS "IX_Planet_Q_R" ON "Planet" (
	"Q",
	"R"
);
CREATE INDEX IF NOT EXISTS "IX_Spacecraft_PlayerId" ON "Spacecraft" (
	"PlayerId"
);
CREATE INDEX IF NOT EXISTS "IX_Spacecraft_Q_R" ON "Spacecraft" (
	"Q",
	"R"
);
CREATE UNIQUE INDEX IF NOT EXISTS "IX_Star_Q_R" ON "Star" (
	"Q",
	"R"
);
CREATE INDEX IF NOT EXISTS "IX_Concept_ArticleValue" ON "Concept" (
	"ArticleValue"
);
CREATE INDEX IF NOT EXISTS "IX_Concept_SubstantiveValue" ON "Concept" (
	"SubstantiveValue"
);
CREATE UNIQUE INDEX IF NOT EXISTS "IX_Resource_Key_Arguments_Value" ON "Resource" (
	"Key",
	"Arguments",
	"Value"
);
CREATE UNIQUE INDEX IF NOT EXISTS "IX_Player_UserId" ON "Player" (
	"UserId"
);
CREATE INDEX IF NOT EXISTS "IX_Navigation_PlayerId" ON "Navigation" (
	"PlayerId"
);
CREATE UNIQUE INDEX IF NOT EXISTS "IX_Navigation_Q_R_PlayerId" ON "Navigation" (
	"Q",
	"R",
	"PlayerId"
);
COMMIT;
