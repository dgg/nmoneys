version = File.read(File.expand_path("../VERSION.Exchange",__FILE__)).strip

Gem::Specification.new do |spec|
  spec.platform    = Gem::Platform::RUBY
  spec.name        = 'nmoneys-exchange'
  spec.version     = version
  spec.files = Dir['lib/**/*'] + Dir['docs/**/*']

  spec.summary     = 'NMoneys Exchange- Extension to NMoneys to support exchange operations'
  spec.description = '
  NMoneys Exchange extends the NMoneys implementation of the Money Value Object to support exchange operations between moneys in different currencies'
  
  spec.authors           = ['Daniel Gonzalez Garcia']
  spec.email             = 'nmoneys@googlegroups.com'
  spec.homepage          = 'http://code.google.com/p/nmoneys/'
  spec.rubyforge_project = 'nmoneys-exchange'

  spec.add_dependency('nmoneys', '>= 2.0.0.0')
end