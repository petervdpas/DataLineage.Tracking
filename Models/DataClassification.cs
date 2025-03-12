using System;
using DataLineage.Enums;

namespace DataLineage.Tracking.Models
{
    /// <summary>
    /// Represents the CIA (Confidentiality, Integrity, Availability) classification levels for data.
    /// Used to determine the security, reliability, and accessibility requirements of a dataset.
    /// </summary>
    public class DataClassification
    {
        /// <summary>
        /// Defines the confidentiality level of the data.
        /// Ensures that only authorized entities have access.
        /// </summary>
        public ConfidentialityLevel Confidentiality { get; set; }

        /// <summary>
        /// Defines the integrity level of the data.
        /// Ensures that data remains unaltered and trustworthy.
        /// </summary>
        public IntegrityLevel Integrity { get; set; }

        /// <summary>
        /// Defines the availability level of the data.
        /// Ensures that data is accessible when needed.
        /// </summary>
        public AvailabilityLevel Availability { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="DataClassification"/> with specified levels.
        /// </summary>
        /// <param name="confidentiality">The confidentiality level.</param>
        /// <param name="integrity">The integrity level.</param>
        /// <param name="availability">The availability level.</param>
        public DataClassification(
            ConfidentialityLevel confidentiality = ConfidentialityLevel.Medium,
            IntegrityLevel integrity = IntegrityLevel.Medium,
            AvailabilityLevel availability = AvailabilityLevel.Medium)
        {
            Confidentiality = confidentiality;
            Integrity = integrity;
            Availability = availability;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DataClassification"/> using a three-digit integer.
        /// </summary>
        /// <param name="classificationCode">A number like 111, 222, 333 representing CIA levels.</param>
        /// <exception cref="ArgumentException">Thrown if the number is not valid (111-333).</exception>
        public DataClassification(int classificationCode)
        {
            var classification = ParseFromInt(classificationCode);
            Confidentiality = classification.Confidentiality;
            Integrity = classification.Integrity;
            Availability = classification.Availability;
        }

        /// <summary>
        /// Converts a <see cref="DataClassification"/> to a three-digit integer.
        /// </summary>
        /// <param name="classification">The classification object.</param>
        public static implicit operator int(DataClassification classification)
            => ((int)classification.Confidentiality * 100) +
               ((int)classification.Integrity * 10) +
               (int)classification.Availability;

        /// <summary>
        /// Converts a three-digit integer into a <see cref="DataClassification"/> object.
        /// </summary>
        /// <param name="classificationCode">A number like 111, 222, 333 representing CIA levels.</param>
        public static implicit operator DataClassification(int classificationCode)
            => new(classificationCode);

        /// <summary>
        /// Parses a three-digit integer into a <see cref="DataClassification"/> object.
        /// Ensures all digits are between 1 and 3.
        /// </summary>
        /// <param name="classificationCode">A number like 111, 222, 333 representing CIA levels.</param>
        /// <returns>A <see cref="DataClassification"/> object with the parsed values.</returns>
        /// <exception cref="ArgumentException">Thrown if any digit is not between 1 and 3.</exception>
        public static DataClassification ParseFromInt(int classificationCode)
        {
            if (classificationCode < 111 || classificationCode > 333)
            {
                throw new ArgumentException("Invalid classification code. Use values between 111 and 333.");
            }

            // Extract digits
            int c = classificationCode / 100 % 10; // First digit (Confidentiality)
            int i = classificationCode / 10 % 10;  // Second digit (Integrity)
            int a = classificationCode % 10;       // Third digit (Availability)

            // Validate each digit (must be 1, 2, or 3)
            if (c is < 1 or > 3 || i is < 1 or > 3 || a is < 1 or > 3)
            {
                throw new ArgumentException("Each digit in the classification code must be 1, 2, or 3.");
            }

            return new DataClassification((ConfidentialityLevel)c, (IntegrityLevel)i, (AvailabilityLevel)a);
        }

        /// <summary>
        /// Returns the CIA classification as a formatted string (e.g., "C2-I3-A1").
        /// </summary>
        /// <returns>A string representation of the data classification.</returns>
        public override string ToString() => $"C{(int)Confidentiality}-I{(int)Integrity}-A{(int)Availability}";
    }
}