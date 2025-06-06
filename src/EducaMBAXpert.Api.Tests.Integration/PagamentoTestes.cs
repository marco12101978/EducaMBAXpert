using EducaMBAXpert.Api.Tests.Integration.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EducaMBAXpert.Api.Tests.Integration
{
    [TestCaseOrderer("EducaMBAXpert.Api.Tests.Integration.Config.PriorityOrderer", "EducaMBAXpert.Api.Tests.Integration")]
    [Collection(nameof(IntegrationWebTestsFixtureCollection))]
    public class PagamentoTestes
    {
        private readonly IntegrationTestsFixture<Program> _testsFixture;
        public PagamentoTestes(IntegrationTestsFixture<Program> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact]
        public async Task HttpPost_api_v1_pagamentos_pagamento()
        {

        }

        [Fact]
        public async Task HttpGet_api_v1_pagamentos_obter_todos()
        {

        }

        [Fact]
        public async Task HttpGet_api_v1_pagamentos_obter_id_guid()
        {

        }
    }
}
