import { createClient } from "@supabase/supabase-js";

const supabaseUrl = import.meta.env.VITE_SUPABASE_URL;
const supabaseKey = import.meta.env.VITE_SUPABASE_SERVICE_KEY;

const supabase = createClient(supabaseUrl, supabaseKey);

export const findUserEmail = async (username) => {
	const { data, error } = await supabase
		.from("profiles")
		.select("email")
		.eq("username", username)
		.single();
	if (!error) {
		return data.email;
	}
};

export const getUsers = async () => {
	const { data, error } = await supabase.from("profiles").select("*");

	return { data, error };
};

export const getSessionToken = async () => {
	const { data: session, error } = await supabase.auth.getSession();
	if (!error) {
		return session.session.access_token;
	}
};

export const findUserById = async (id) => {
	const { data, error } = await supabase
		.from("profiles")
		.select("full_name")
		.eq("id", id)
		.single();
	if (!error) {
		return data.full_name;
	}
};

export const createUser = async (email, password, fullName, userName) => {
	const newUser = await supabase.functions.invoke("createNewUser", {
		body: {
			email,
			password,
			fullname: fullName,
			username: userName,
		},
	});
	console.log(newUser);
	return newUser;
};

export default supabase;
