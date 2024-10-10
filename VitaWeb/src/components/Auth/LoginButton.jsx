import supabase from "../../services/Supabase";

export default function LoginButton() {
	const login = () =>
		supabase.auth.signInWithPassword({
			email: "askelysgaard",
			password: "Test1234",
		});

	return (
		<button
			className="w-40 h-10 rounded text-white bg-blue-600 shadow-depth_blue active:shadow hover:bg-blue-700"
			onClick={() => {
				login();
				window.location.reload();
			}}
		>
			Log Ind
		</button>
	);
}
