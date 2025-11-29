import {
  Button,
  TextField,
  Typography,
  Container,
  Box,
  Grid,
  Link,
} from "@mui/material";
import { Form, useActionData, useNavigation } from "react-router";
import { Link as RouterLink } from "react-router";

const LoginPage = () => {
  const navigation = useNavigation();
  const actionData = useActionData() as { error: string } | undefined;
  const isSubmitting = navigation.state === "submitting";

  return (
    <Container maxWidth="xs">
      <Box
        sx={{
          marginTop: 8,
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        <Typography component="h1" variant="h5">
          Sign in
        </Typography>
        {actionData && actionData.error && (
          <Typography color="error">{actionData.error}</Typography>
        )}
        <Form method="post">
          <TextField
            margin="normal"
            required
            fullWidth
            id="email"
            label="Email Address"
            name="email"
            autoComplete="email"
            autoFocus
          />
          <TextField
            margin="normal"
            required
            fullWidth
            name="password"
            label="Password"
            type="password"
            id="password"
            autoComplete="current-password"
          />
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
            disabled={isSubmitting}
          >
            {isSubmitting ? "Signing In..." : "Sign In"}
          </Button>
          <Grid container>
            <Grid>
              <Link
                component={RouterLink}
                to="/forgot-password"
                variant="body2"
              >
                Reset password?
              </Link>
            </Grid>
            <Grid>
              <Link component={RouterLink} to="/register" variant="body2">
                {"Don't have an account? Sign Up"}
              </Link>
            </Grid>
          </Grid>
        </Form>
      </Box>
    </Container>
  );
};

export default LoginPage;
