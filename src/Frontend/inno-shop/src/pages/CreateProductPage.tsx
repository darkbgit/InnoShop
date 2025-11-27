import { Button } from "@mui/material";
import { Form, Link, useActionData, useNavigation } from "react-router";

const CreateProductPage = () => {
  const actionData = useActionData() as { error: string } | undefined;

  const errors = 1;
  const navigation = useNavigation();
  const isSubmitting = navigation.state === "submitting";
  return (
    <div className="create-page-container">
      <h2> Create new product</h2>
      {errors && <div className="error-banner">{errors}</div>}
      <Form method="post">
        <div className="actions">
          <Link to="/" className="btn-cancel">
            Cancel
          </Link>
          <Button type="submit" disabled={isSubmitting}>
            {isSubmitting ? "Creating..." : "Save product"}
          </Button>
        </div>
      </Form>
    </div>
  );
};

export default CreateProductPage;
