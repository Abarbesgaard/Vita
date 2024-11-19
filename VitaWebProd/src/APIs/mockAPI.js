import { createServer, Model, Factory } from "miragejs";
import { faker } from "@faker-js/faker";

export function makeServer() {
	createServer({
		models: {
			notice: Model,
			video: Model,
		},

		factories: {
			notice: Factory.extend({
				title() {
					return faker.food.dish();
				},
				content() {
					return faker.food.description();
				},
				createdAt() {
					return faker.date.recent({ days: 10 });
				},
			}),
		},

		seeds(server) {
			server.createList("notice", 5);
			server.create("video", {
				title: "Test video 1",
				url: "https://www.youtube.com/watch?v=JGwWNGJdvx8",
				description: "Beskrivelse af video 1",
			});
			server.create("video", {
				title: "Test Video 2",
				url: "https://www.youtube.com/watch?v=JGwWNGJdvx8",
				description: "Kort Beskrivelse",
			});
			server.create("video", {
				title: "Test Video 3 med lang titel",
				url: "https://www.youtube.com/watch?v=JGwWNGJdvx8",
				description:
					"En lang beskrivelse af video 3 for at teste om det virker og hvordan det ser ud på UI. Denne beskrivelse er virkelig lang og fylder meget.",
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
		},
	});
}
