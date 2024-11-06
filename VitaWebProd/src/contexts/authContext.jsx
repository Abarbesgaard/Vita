import { createContext, useState, useEffect } from "react";
import supabase, { findUserEmail } from "../services/supabase";
import { ImSpinner2 } from "react-icons/im";
import { IconContext } from "react-icons";

const AuthContext = createContext({ user: null });

const AuthProvider = ({ children }) => {
	const [user, setUser] = useState(null);
	const [session, setSession] = useState(null);
	const [loading, setLoading] = useState(true);
	console.log("session", session);

	useEffect(() => {
		const { data: listener } = supabase.auth.onAuthStateChange(
			(event, session) => {
				console.log("session onAuthStateChange", session);
				setSession(session);
				setUser(session?.user ?? null);
				setLoading(false);
			}
		);
		return () => {
			listener?.subscription.unsubscribe();
		};
	}, []);

	const signIn = async (username, password) => {
		setLoading(true);
		const email = await findUserEmail(username);
		const { data, error } = await supabase.auth.signInWithPassword({
			email,
			password,
		});
		setLoading(false);
		return { data, error };
	};

	const signOut = async () => {
		setLoading(true);
		const { error } = await supabase.auth.signOut();
		if (!error) {
			setUser(null);
			setSession(null);
		}
		setLoading(false);
		return { error };
	};

	return (
		<AuthContext.Provider value={{ user, signIn, signOut }}>
			{!loading ? (
				children
			) : (
				<IconContext.Provider value={{ size: "2em" }}>
					<div className="w-full h-dvh text-center flex items-center justify-center">
						<ImSpinner2 className="animate-spin" />
					</div>
				</IconContext.Provider>
			)}
		</AuthContext.Provider>
	);
};

export { AuthProvider, AuthContext };
