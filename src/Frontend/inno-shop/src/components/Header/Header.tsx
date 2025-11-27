import { Box, Typography } from "@mui/material";
import LoginMenu from "../LoginMenu/LoginMenu";
import { Link, useRouteLoaderData } from "react-router";
import type { UserInfo } from "../../interfaces/user.interface";

const Header = () => {
  const user = useRouteLoaderData("root") as UserInfo | null;
  return (
    <div>
      <Box p={2}>
        <nav style={{ display: "flex", gap: "1rem", alignItems: "center" }}>
          <Link to="/">Home</Link>
          {user && user.roles.includes("admin") && (
            <Typography>{user.email}</Typography>
          )}
          <LoginMenu />
        </nav>
      </Box>
      <hr />
    </div>
  );
};

export default Header;
