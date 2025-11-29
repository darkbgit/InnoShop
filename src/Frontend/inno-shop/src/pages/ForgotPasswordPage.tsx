import { Button, TextField, Typography, Container, Box } from "@mui/material";
import { Form, useActionData, useNavigation } from "react-router";

const ForgotPasswordPage = () => {
  const navigation = useNavigation();
  const actionData = useActionData() as { error?: string; message?: string };
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
          Forgot Password
        </Typography>
        <Typography component="p" variant="body2" sx={{ mt: 2, textAlign: "center" }}>
          Enter your email address and we will send you a link to reset your
          password.
        </Typography>
        {actionData?.error && (
          <Typography color="error" sx={{ mt: 2 }}>
            {actionData.error}
          </Typography>
        )}
        {actionData?.message && (
          <Typography color="primary" sx={{ mt: 2 }}>
            {actionData.message}
          </Typography>
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
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
            disabled={isSubmitting || !!actionData?.message}
          >
            {isSubmitting ? "Sending..." : "Send Password Reset Email"}
          </Button>
        </Form>
      </Box>
    </Container>
  );
};

export default ForgotPasswordPage;
