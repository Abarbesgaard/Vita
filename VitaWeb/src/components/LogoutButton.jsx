import { useAuth0 } from "@auth0/auth0-react";

export default function LogoutButton() {
	const { logout } = useAuth0();

	return (
		<button
			className="w-40 h-10 text-white bg-red-600 rounded shadow-depth_red hover:bg-red-700 active:shadow"
			onClick={() =>
				logout({
					logoutParams: {
						returnTo: window.location.origin,
					},
				})
			}
		>
			Log Ud
		</button>
	);
}
