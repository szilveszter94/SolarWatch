import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faFacebook,
  faTwitter,
  faInstagram,
  faLinkedin,
} from "@fortawesome/free-brands-svg-icons";
import "./Footer.scss";

const Footer = () => {
  return (
    <footer className="text-center">
      <div className="container">
        <div className="row mt-3">
          <div className="col-md-12">
            <p className="footer-text">
              &copy; {new Date().getFullYear()}{" "}
              <span className="brand-name fs-5">{"SolarWatch "}</span>. All Rights
              Reserved.
            </p>
          </div>
          <div className="col-md-12">
            <ul className="list-inline">
              <li className="list-inline-item me-3">
                <a href="https://www.facebook.com">
                  <FontAwesomeIcon icon={faFacebook} />
                </a>
              </li>
              <li className="list-inline-item me-3">
                <a href="https://www.twitter.com">
                  <FontAwesomeIcon icon={faTwitter} />
                </a>
              </li>
              <li className="list-inline-item me-3">
                <a href="https://www.instagram.com">
                  <FontAwesomeIcon icon={faInstagram} />
                </a>
              </li>
              <li className="list-inline-item">
                <a href="https://www.linkedin.com">
                  <FontAwesomeIcon icon={faLinkedin} />
                </a>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
