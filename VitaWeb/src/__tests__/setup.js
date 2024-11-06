import "@testing-library/jest-dom/vitest";
import { useAuth } from "../context/AuthContext";

vi.mock("../context/AuthContext");
useAuth.mockImplementation(() => {
	return {
		user: null,
		signIn: vi.fn(),
		signOut: vi.fn(),
	};
});


