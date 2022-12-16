import "../static/home.css";
import ReviewMiniature from "../components/ReviewMiniature";
import FeaturedReview from "../components/FeaturedReview";
import "react-circular-progressbar/dist/styles.css";

export default function Home() {
  return (
    <div className="home">
      <FeaturedReview></FeaturedReview>
      <h2>Latest reviews</h2>
      <ReviewMiniature />
    </div>
  );
}
