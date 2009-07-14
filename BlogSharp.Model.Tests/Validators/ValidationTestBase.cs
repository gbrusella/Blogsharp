// <copyright file="ValidationTestBase.cs" company="BlogSharp">
// Apache Licence 2.0 
// </copyright>
// <author>Gonzalo Brusella</author>
// <email>gonzalo@brusella.com.ar</email>
// <date>2009-02-21</date>

namespace BlogSharp.Model.Tests.Validators
{
    using System;
    using System.Linq.Expressions;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;

    /// <summary>
    /// Base for all validators test
    /// </summary>
    /// <typeparam name="TValidator">The validator</typeparam>
    /// <typeparam name="TValidatee">The validated</typeparam>
    [TestFixture]
    public class ValidationTestBase<TValidator, TValidatee>
        where TValidator : AbstractValidator<TValidatee>, new()
        where TValidatee : class, new()
    {
        /// <summary>
        /// The validator to run.
        /// </summary>
        private TValidator validator;

        #region Setup/Teardown
        /// <summary>
        /// The SetUp of all the tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.validator = new TValidator();
        }
        #endregion
 
        /// <summary>
        /// Generic function to validate an error returning function.
        /// </summary>
        /// <typeparam name="TValue">The genetic value.</typeparam>
        /// <param name="expression">The errer expression.</param>
        /// <param name="value">The comparing value.</param>
        /// <exception cref="ArgumentNullException"><c>expression</c> is null.</exception>
        protected virtual void ShouldHaveErrors<TValue>(Expression<Func<TValidatee, TValue>> expression, TValue value)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            this.validator.ShouldHaveValidationErrorFor(expression, value);
        }

        /// <summary>
        /// Generic function to validate an non error returning function.
        /// </summary>
        /// <typeparam name="TValue">The genetic value.</typeparam>
        /// <param name="expression">The errer expression.</param>
        /// <param name="value">The comparing value.</param>
        protected virtual void ShouldNotHaveErrors<TValue>(Expression<Func<TValidatee, TValue>> expression, TValue value)
        {
            this.validator.ShouldNotHaveValidationErrorFor(expression, value);
        }
    }
}