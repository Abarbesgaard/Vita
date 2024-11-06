import { createServer, Model, Factory } from "miragejs";
import { faker } from "@faker-js/faker";

export function makeServer() {
	createServer({
		models: {
			notice: Model,
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
		},
	});
}
