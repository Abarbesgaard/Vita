import { expect, test } from "vitest";
import { render } from "@testing-library/react";
import NonUserCard from "../components/NonUserCard";

test("NonUserCard renders correctly", () => {
	const card = render(<NonUserCard />);
	const picture = card.getByTestId("defaultAvatar");
	expect(picture.src).toContain(
		"https://upload.wikimedia.org/wikipedia/commons/a/ac/Default_pfp.jpg"
	);
});
