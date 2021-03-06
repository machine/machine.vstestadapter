using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Diagnostics;
using Machine.VSTestAdapter.Discovery;

namespace Machine.VSTestAdapter.Helpers
{
    public static class SpecTestHelper
    {
        public static TestCase GetVSTestCaseFromMSpecTestCase(string source, MSpecTestCase mspecTestCase, bool disableFullTestNames, Uri testRunnerUri)
        {
            VisualStudioTestIdentifier vsTest = mspecTestCase.ToVisualStudioTestIdentifier();

            TestCase testCase = new TestCase(vsTest.FullyQualifiedName, testRunnerUri, source)
            {
                DisplayName = disableFullTestNames ? mspecTestCase.SpecificationDisplayName : $"{mspecTestCase.ContextDisplayName} it {mspecTestCase.SpecificationDisplayName}",
                CodeFilePath = mspecTestCase.CodeFilePath,
                LineNumber = mspecTestCase.LineNumber
            };

            Trait classTrait = new Trait("ClassName", mspecTestCase.ClassName);
            Trait subjectTrait = new Trait("Subject", string.IsNullOrEmpty(mspecTestCase.Subject) ? "No Subject" : mspecTestCase.Subject);

            testCase.Traits.Add(classTrait);
            testCase.Traits.Add(subjectTrait);

            if (mspecTestCase.Tags != null)
            {
                foreach (var tag in mspecTestCase.Tags)
                {
                    if (!string.IsNullOrEmpty(tag))
                    {
                        Trait tagTrait = new Trait("Tag", tag);
                        testCase.Traits.Add(tagTrait);
                    }
                }
            }

            if (!string.IsNullOrEmpty(mspecTestCase.BehaviorFieldName))
                testCase.Traits.Add(new Trait("BehaviorField", mspecTestCase.BehaviorFieldName));

            if (!string.IsNullOrEmpty(mspecTestCase.BehaviorFieldType))
                testCase.Traits.Add(new Trait("BehaviorType", mspecTestCase.BehaviorFieldType));

            Debug.WriteLine($"TestCase {testCase.FullyQualifiedName}");
            return testCase;
        }
    }

}
