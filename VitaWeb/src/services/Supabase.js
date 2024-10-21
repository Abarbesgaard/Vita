import { createClient } from "@supabase/supabase-js";

const supabaseUrl = import.meta.env.VITE_SUPABASE_URL;
const supabaseKey = import.meta.env.VITE_SUPABASE_SERVICE_KEY;

const supabase = createClient(supabaseUrl, supabaseKey);

export const getUsers = async () => {
	return await supabase.auth.admin.listUsers();
};

export const getSessionToken = async () => {
	const { data: session, error } = await supabase.auth.getSession();
	if (!error) {
		return session.session.access_token;
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
