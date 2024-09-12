import { SignJWT } from "jose";

// const createJWT = async (payload) => {
// 	const secret = createSecretKey("secret1", "utf-8");
// 	const alg = "HS256";

// 	const token = await new SignJWT({ sub: payload })
// 		.setProtectedHeader({ alg, typ: "JWT" })
// 		.sign(secret);

// 	console.log(token);
// 	return token;
// };

export const getAllVideos = async (userId) => {
	try {
		const response = await fetch("http://localhost:5039/videos/getall", {
			headers: {
				Authorization:
					"Bearer " +
					"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhdXRoMHw2NmM4NTliM2EwNjY4NmMzNDQwZjEyYWEifQ.LWIkdPCPNKW1cdXn_TJgYIWxqOY0fUgCEln0kAhhEwU",
			},
		}).then((response) => response.json());

		return response;
	} catch (error) {
		return error;
	}
};

export const saveVideo = async (video, userId) => {
	console.log(JSON.stringify(video));
	try {
		const response = await fetch("http://localhost:5039/videos/create", {
			method: "POST",
			headers: {
				Authorization:
					"Bearer " +
					"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhdXRoMHw2NmM4NTliM2EwNjY4NmMzNDQwZjEyYWEifQ.LWIkdPCPNKW1cdXn_TJgYIWxqOY0fUgCEln0kAhhEwU",
				"Content-Type": "application/json",
				charset: "utf-8",
			},
			body: JSON.stringify(video),
		}).then((response) => response.json());

		return response;
	} catch (error) {
		return error;
	}
};

export const deleteVideoFromDb = async (id) => {
	try {
		const response = await fetch(`http://localhost:5039/videos/delete/${id}`, {
			method: "DELETE",
			headers: {
				Authorization:
					"Bearer " +
					"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhdXRoMHw2NmM4NTliM2EwNjY4NmMzNDQwZjEyYWEifQ.LWIkdPCPNKW1cdXn_TJgYIWxqOY0fUgCEln0kAhhEwU",
				"Content-Type": "application/json",
				charset: "utf-8",
			},
		}).then((response) => response.json());

		return response;
	} catch (error) {
		return error;
	}
};
