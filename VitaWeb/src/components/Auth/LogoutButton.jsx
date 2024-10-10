import { useAuth } from "../../context/AuthContext";

export default function LogoutButton() {
	const { signOut } = useAuth();

	// Dette er en kommentar
	return (
		<button
			className="w-40 h-10 mt-auto text-white bg-red-600 rounded shadow-depth_red hover:bg-red-700 active:shadow"
			onClick={async () => {
				await signOut();
			}}
		>
			Log Ud
		</button>
	);
}
