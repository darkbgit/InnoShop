import {
  Alert,
  Box,
  CircularProgress,
  Container,
  Typography,
} from "@mui/material";
import { useLoaderData, Link as RouterLink } from "react-router";

const ConfirmEmailPage = () => {
  const loaderData = useLoaderData() as { success: boolean; message: string };

  return (
    <Container maxWidth="xs">
      <Box
        sx={{
          marginTop: 8,
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          textAlign: "center",
        }}
      >
        <Typography component="h1" variant="h5" sx={{ mb: 2 }}>
          Email Confirmation
        </Typography>
        {!loaderData && <CircularProgress />}
        {loaderData && (
          <Alert
            severity={loaderData.success ? "success" : "error"}
            sx={{ width: "100%", mt: 2 }}
          >
            {loaderData.message}
          </Alert>
        )}
        {loaderData?.success && (
          <Typography sx={{ mt: 2 }}>
            You can now <RouterLink to="/login">log in</RouterLink>.
          </Typography>
        )}
      </Box>
    </Container>
  );
};

export default ConfirmEmailPage;
