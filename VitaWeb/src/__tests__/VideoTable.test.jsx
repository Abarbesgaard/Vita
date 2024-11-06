import { beforeEach, afterEach, it, expect, describe, vi } from "vitest";
import { render, screen } from "@testing-library/react";
import VideoPage from "../pages/VideoPage";

describe("VideoTable", () => {
	it("should", () => {
		render(<VideoPage />);
	});

	screen.debug();
});
