import { useState } from "react";
import { useAuth } from "../context/AuthContext";
import { Navigate } from "react-router-dom";

const Login = () => {
	const [email, setEmail] = useState("");
	const [password, setPassword] = useState("");
	const { user, signIn } = useAuth();

	const handleSubmit = (e) => {
		e.preventDefault();
		signIn(email, password);
	};

	if (user) {
		return <Navigate to="/" replace />;
	}

	if (!user) {
		return (
			<div className="flex items-center justify-center min-h-screen bg-gray-100">
				<div className="w-full max-w-md p-8 space-y-6 bg-white rounded shadow-md">
					<h2 className="text-2xl font-bold text-center">Login</h2>
					<form onSubmit={handleSubmit} className="space-y-4">
						<div>
							<label
								htmlFor="email"
								className="block text-sm font-medium text-gray-700"
							>
								Email
							</label>
							<input
								type="email"
								id="email"
								value={email}
								onChange={(e) => setEmail(e.target.value)}
								required
								className="w-full px-3 py-2 mt-1 border rounded-md focus:outline-none focus:ring focus:ring-indigo-200"
							/>
						</div>
						<div>
							<label
								htmlFor="password"
								className="block text-sm font-medium text-gray-700"
							>
								Password
							</label>
							<input
								type="password"
								id="password"
								value={password}
								onChange={(e) => setPassword(e.target.value)}
								required
								className="w-full px-3 py-2 mt-1 border rounded-md focus:outline-none focus:ring focus:ring-indigo-200"
							/>
						</div>
						<button
							type="submit"
							className="w-full px-4 py-2 font-bold text-white bg-indigo-600 rounded-md hover:bg-indigo-700 focus:outline-none focus:ring focus:ring-indigo-200"
						>
							Login
						</button>
					</form>
				</div>
			</div>
		);
	}
};

export default Login;
