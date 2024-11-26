import { createServer, Model, Factory } from "miragejs";
import { faker } from "@faker-js/faker";

export function makeServer() {
	createServer({
		models: {
			notice: Model,
			video: Model,
		},

		// factories: {
		// 	notice: Factory.extend({
		// 		title() {
		// 			return faker.food.dish();
		// 		},
		// 		content() {
		// 			return faker.food.description();
		// 		},
		// 		createdAt() {
		// 			return faker.date.recent({ days: 10 });
		// 		},
		// 	}),
		// },

		seeds(server) {
			// server.createList("notice", 5);
			server.create("notice", {
				title: "Filmaften i fællesstuen",
				content:
					"Hej alle! Husk, at vi holder filmaften i fællesstuen på fredag kl. 19:00. Vi har popcorn klar, og I kan være med til at vælge filmen. Kom og hyg jer med os!",
				createdAt: faker.date.recent({ days: 10 }),
			});
			server.create("notice", {
				title: "Vandafbrydelse onsdag",
				content:
					"Kære beboere, onsdag den 29. november bliver vandet midlertidigt lukket mellem kl. 10:00 og 13:00 på grund af vedligeholdelse. Husk at fylde vandflasker, hvis I får brug for det i løbet af formiddagen.",
				createdAt: faker.date.recent({ days: 10 }),
			});
			server.create("notice", {
				title: "Husk at rydde op efter jer selv",
				content:
					"Hej alle! Vi vil gerne minde jer om, at det er vigtigt at rydde op efter jer selv i køkkenet og fællesområderne. På den måde skaber vi et hyggeligt miljø for alle. Tak for jeres hjælp!",
				createdAt: faker.date.recent({ days: 10 }),
			});
			server.create("notice", {
				title: "Mødeaften med beboerne",
				content:
					"Hej allesammen! Vi inviterer jer til en hyggelig mødeaften på tirsdag kl. 18:00 i fællesstuen. Her kan I komme med forslag til aktiviteter og give jeres mening til kende om botilbuddet. Vi glæder os til at høre fra jer!",
				createdAt: faker.date.recent({ days: 10 }),
			});
			server.create("notice", {
				title: "Fødselsdagsfejring for Jonas",
				content:
					"Hej! På lørdag fejrer vi Jonas’ fødselsdag kl. 14:00 i fælleskøkkenet. Vi sørger for kage og sodavand. Kom og vær med til at fejre Jonas og gøre dagen ekstra speciel!",
				createdAt: faker.date.recent({ days: 10 }),
			});
			server.create("video", {
				title: "Lær at håndtere stress i hverdagen",
				description:
					"Denne video guider dig gennem teknikker til at reducere stress og finde ro i en travl hverdag. Perfekt til de dage, hvor alt føles overvældende.",
				url: "https://www.youtube.com/watch?v=GKlzI9aVmGk",
			});
			server.create("video", {
				title: "Tips til en bedre nattesøvn",
				description:
					"Få praktiske råd til at forbedre din søvnkvalitet, herunder hvordan du skaber en god aftenrutine og undgår søvnforstyrrelser.",
				url: "https://www.youtube.com/watch?v=hGfW3Tj5VNk",
			});
			server.create("video", {
				title: "Sådan kan du håndtere angst",
				description:
					"Denne video forklarer, hvad angst er, og giver dig konkrete øvelser, du kan bruge til at mindske angstsymptomer i øjeblikket.",
				url: "https://www.youtube.com/watch?v=7-xVVGvEYm8",
			});
		},

		routes() {
			this.get("/api/notices", (schema) => {
				return schema.notices.all();
			});

			this.post("/api/notices", (schema, request) => {
				return schema.notices.create({
					title: faker.food.dish(),
					content: faker.food.description(),
					createdAt: faker.date.recent({ days: 10 }),
				});
			});

			this.get("/api/videos", (schema) => {
				return schema.videos.all();
			});

			this.post("/api/videos", (schema, request) => {
				const attrs = JSON.parse(request.requestBody);
				return schema.videos.create(attrs);
			});

			this.put("/api/videos/:id", (schema, request) => {
				const id = request.params.id;
				const attrs = JSON.parse(request.requestBody);
				const video = schema.videos.find(id);
				return video.update({
					title: attrs.title,
					url: attrs.url,
					description: attrs.description,
				});
			});

			this.delete("/api/videos/:id", (schema, request) => {
				const id = request.params.id;
				schema.videos.find(id).destroy();
			});

			this.passthrough("https://localhost:8081/**");
		},
	});
}
